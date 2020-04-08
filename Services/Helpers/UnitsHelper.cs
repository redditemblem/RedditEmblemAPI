using NCalc;
using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Services.Helpers
{
    public class UnitsHelper : Helper
    {
        private static Regex unitNumberRegex = new Regex(@"\s([0-9]+$)"); //matches digits at the end of a string (ex. "Swordmaster _05_")
        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        private static Regex unitStatRegex = new Regex(@"{UnitStat\[([A-Za-z]+)\]}"); //match unit stat name
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat[A-Za-z]+]}"); //match weapon stat name

        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static IList<Unit> Process(UnitsConfig config, IList<Item> items, IList<Skill> skills, IDictionary<string, Class> classes, List<List<Tile>> map)
        {
            IList<Unit> units = new List<Unit>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();

                    //Skip blank units
                    if (string.IsNullOrEmpty(unit.ElementAtOrDefault(config.UnitName)))
                        continue;

                    Unit temp = new Unit()
                    {
                        Name = unit.ElementAtOrDefault(config.UnitName) ?? string.Empty,
                        SpriteURL = unit.ElementAtOrDefault(config.SpriteURL) ?? string.Empty,
                        Coordinates = new Coordinate(unit.ElementAtOrDefault(config.Coordinates) ?? string.Empty),
                        UnitSize = OptionalSafeIntParse(unit.ElementAtOrDefault(config.UnitSize) ?? string.Empty, "Unit Size", true, 1),
                        HasMoved = ((unit.ElementAtOrDefault(config.HasMoved) ?? string.Empty) == "Yes"),
                        HP = new HP((unit.ElementAtOrDefault(config.CurrentHP) ?? string.Empty),
                                    (unit.ElementAtOrDefault(config.MaxHP) ?? string.Empty)),
                        Level = SafeIntParse(unit.ElementAtOrDefault(config.Level), "Level", true),
                        Affiliation = unit.ElementAtOrDefault(config.Affiliation) ?? string.Empty,
                        Experience = SafeIntParse(unit.ElementAtOrDefault(config.Experience) ?? string.Empty, "Experience", true) % 100,
                        HeldCurrency = SafeIntParse(unit.ElementAtOrDefault(config.HeldCurrency) ?? "-1", "Currency", false)
                    };

                    //Find unit number
                    Match numberMatch = unitNumberRegex.Match(temp.Name);
                    if (numberMatch.Success)
                        temp.UnitNumber = numberMatch.Value.Trim();

                    //Add items to lists and dictionaries
                    BuildTextFieldList(temp, unit, config.TextFields);
                    BuildTagsList(temp, (unit.ElementAtOrDefault(config.Tags) ?? string.Empty));
                    BuildStatsDictionary(temp, unit, config.Stats);
                    BuildInventory(temp, unit, config.Inventory, items);
                    BuildSkills(temp, unit, config.Skills, skills);
                    BuildClassesList(temp, unit, config.Classes, classes);

                    ApplyInventoryItemModifiers(temp);
                    CalculateCombatStats(temp, config.CalculatedStats);
                    AddUnitToMap(temp, map);

                    units.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(row.ElementAtOrDefault(config.UnitName).ToString(), ex);
                }
            }

            return units;
        }

        private static void BuildTextFieldList(Unit unit, IList<string> data, IList<int> configFields)
        {
            foreach (int field in configFields)
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault(field)))
                    unit.TextFields.Add(data.ElementAtOrDefault(field));
        }

        private static void BuildClassesList(Unit unit, IList<string> data, IList<int> configFields, IDictionary<string, Class> classes)
        {
            foreach (int field in configFields)
            {
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault(field)))
                {
                    Class match;
                    if (!classes.TryGetValue(data.ElementAt(field), out match))
                        throw new UnmatchedClassException(data.ElementAt(field));

                    match.Matched = true;
                    unit.ClassList.Add(match);

                    //Append class tags to unit's tags
                    unit.Tags = unit.Tags.Union(match.Tags).Distinct().ToList();
                }
            }
        }
        
        private static void BuildTagsList(Unit unit, string tagsCSV)
        {
            foreach (string tag in tagsCSV.Split(','))
                if(!string.IsNullOrEmpty(tag))
                    unit.Tags.Add(tag.Trim());
        }

        private static void BuildStatsDictionary(Unit unit, IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            foreach (ModifiedNamedStatConfig s in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse base value
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(s.BaseValue), out val) || val < 0)
                    throw new PositiveIntegerException(s.SourceName, data.ElementAtOrDefault(s.BaseValue) ?? string.Empty);
                temp.BaseValue = val;

                //Parse modifiers list
                foreach (NamedStatConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(data.ElementAtOrDefault(mod.Value), out val))
                        throw new AnyIntegerException(string.Format("{0} {1}", s.SourceName, mod.SourceName), data.ElementAtOrDefault(mod.Value) ?? string.Empty);

                    if(val != 0)
                        temp.Modifiers.Add(mod.SourceName, val);
                }

                unit.Stats.Add(s.SourceName, temp);
            }
        }

        /// <summary>
        /// Adds <c>Item</c> objects from <paramref name="items"/> to <paramref name="unit"/>'s <c>Inventory</c> when their name matches a <paramref name="data"/> cell's value. Also flags items as droppable/equippable and parses uses.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="data"></param>
        /// <param name="config"></param>
        /// <param name="items"></param>
        /// <exception cref="UnmatchedItemException"></exception>
        private static void BuildInventory(Unit unit, IList<string> data, InventoryConfig config, IList<Item> items)
        {
            foreach (int slot in config.Slots)
            {
                string name = data.ElementAtOrDefault(slot);
                if (string.IsNullOrEmpty(name))
                {
                    unit.Inventory.Add(null);
                    continue;
                }

                bool isDroppable = false;
                int uses = 0;

                //Search for droppable syntax
                Match dropMatch = dropRegex.Match(name);
                if (dropMatch.Success)
                {
                    isDroppable = true;
                    name = dropRegex.Replace(name, string.Empty);
                }

                //Search for uses syntax
                Match usesMatch = usesRegex.Match(name);
                if (usesMatch.Success)
                {
                    //Convert item use synatax to int
                    string u = usesMatch.Value.ToString();
                    u = u.Substring(1, u.Length - 2);
                    uses = int.Parse(u);
                    name = usesRegex.Replace(name, string.Empty);
                }

                name = name.Trim();
                Item itemMatch = items.FirstOrDefault(i => i.Name == name);
                if (itemMatch != null)
                {
                    //If we find a match from the items sheet, deep clone it to the unit and paste in our values
                    Item clone = (Item)itemMatch.Clone();
                    clone.OriginalName = data.ElementAtOrDefault(slot);
                    clone.IsDroppable = isDroppable;
                    clone.Uses = uses;

                    unit.Inventory.Add(clone);
                }
                else throw new UnmatchedItemException(name);
            }

            //Find the equipped item and flag it
            if (!string.IsNullOrEmpty(data.ElementAtOrDefault(config.EquippedItem)))
            {
                Item equipped = unit.Inventory.FirstOrDefault(i => i.OriginalName == data.ElementAt(config.EquippedItem));
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(data.ElementAt(config.EquippedItem));
                equipped.IsEquipped = true;
            }
        }

        /// <summary>
        /// Adds <c>Skill</c> objects from <paramref name="skills"/> to <paramref name="unit"/>'s <c>Skills</c> when their name matches a <paramref name="data"/> cell's value.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="data"></param>
        /// <param name="config"></param>
        /// <param name="skills"></param>
        /// <exception cref="UnmatchedSkillException"></exception>
        private static void BuildSkills(Unit unit, IList<string> data, SkillListConfig config, IList<Skill> skills)
        {
            foreach (int slot in config.Slots)
            {
                //Skip blank cells
                string name = data.ElementAtOrDefault(slot);
                if (string.IsNullOrEmpty(name))
                    continue;

                Skill skillMatch = skills.FirstOrDefault(i => i.Name == name);
                if (skillMatch == null)
                    throw new UnmatchedSkillException(name);
                unit.Skills.Add(skillMatch);
            }
        }

        private static void ApplyInventoryItemModifiers(Unit temp)
        {
            //Equipped item
            Item eqp = temp.Inventory.FirstOrDefault(i => i != null && i.IsEquipped);
            if (eqp != null)
            {
                foreach (string stat in eqp.EquippedStatModifiers.Keys)
                {
                    ModifiedStatValue mods = temp.Stats.GetValueOrDefault(stat);
                    if (mods != null)
                        mods.Modifiers.Add(string.Format("{0} ({1})", eqp.Name, "Eqp"), eqp.EquippedStatModifiers[stat]);
                }
            }

            //Inventory items
            foreach (Item inv in temp.Inventory.Where(i => i != null && !i.IsEquipped))
            {
                foreach (string stat in inv.InventoryStatModifiers.Keys)
                {
                    ModifiedStatValue mods = temp.Stats.GetValueOrDefault(stat);
                    if (mods != null)
                        mods.Modifiers.Add(string.Format("{0} ({1})", inv.Name, "Inv"), inv.InventoryStatModifiers[stat]);
                }
            }
        }

        private static void CalculateCombatStats(Unit temp, IList<CalculatedStatConfig> stats)
        {
            foreach(CalculatedStatConfig stat in stats)
            {
                string equation = stat.Equation;

                //{UnitStat[...]}
                //Replaced by values from the unit.Stats list
                MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
                if (unitStatMatches.Count > 0)
                {
                    foreach(Match match in unitStatMatches)
                        equation = equation.Substring(0, match.Index) + temp.Stats[match.Groups[1].Value].FinalValue + equation.Substring(match.Index + match.Length);
                }

                //{WeaponUtilStat}
                Item equipped = temp.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
                equation = equation.Replace("{WeaponUtilStat}", (equipped != null ? temp.Stats[equipped.UtilizedStat].FinalValue : 0).ToString());

                //{WeaponStat[...]}
                MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
                if (weaponStatMatches.Count > 0)
                {
                    foreach (Match match in weaponStatMatches)
                        equation = equation.Substring(0, match.Index) + (equipped != null ? equipped.Stats[match.Groups[1].Value] : 0) + equation.Substring(match.Index + match.Length);
                }

                //Throw an error if anything remains unparsed
                if (equation.Contains("{") || equation.Contains("}"))
                    throw new Exception("The equation \"" + stat.Equation + "\" contains an unrecognized variable.");

                Expression expression = new Expression(equation);
                temp.CalculatedStats.Add(stat.SourceName, (int)expression.Evaluate());
            }
        }

        private static void AddUnitToMap(Unit unit, List<List<Tile>> map)
        {
            //Ignore hidden units
            if (unit.Coordinates.X < 1 || unit.Coordinates.Y < 1)
                return;

            //Find tile corresponsing to units coordinates
            IList<Tile> row = map.ElementAtOrDefault(unit.Coordinates.Y - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates);
            Tile tile = row.ElementAtOrDefault(unit.Coordinates.X - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates);

            //Two way bind the unit and tile objects
            unit.AnchorTile = tile;
            tile.Unit = unit;
            tile.IsUnitAnchor = true;

            unit.MovementRange.Add(new Coordinate(tile.Coordinate));

            if (unit.UnitSize > 1)
            {
                //Calculate origin tile for multi-tile units
                int anchorOffset = (int)Math.Ceiling(unit.UnitSize / 2.0m) - 1;

                for(int y = 0; y < unit.UnitSize; y++)
                {
                    for (int x = 0; x < unit.UnitSize; x++)
                    {
                        IList<Tile> intersectRow = map.ElementAtOrDefault(unit.Coordinates.Y + y - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates.X + x, unit.Coordinates.Y + y);
                        Tile intersectTile = intersectRow.ElementAtOrDefault(unit.Coordinates.X + x - 1) ?? throw new UnitTileOutOfBoundsException(unit.Coordinates.X + x, unit.Coordinates.Y + y);

                        intersectTile.Unit = unit;
                        if(!unit.MovementRange.Contains(intersectTile.Coordinate))
                            unit.MovementRange.Add(intersectTile.Coordinate);
                        
                        if (x == anchorOffset && y == anchorOffset)
                        {
                            unit.OriginTile = intersectTile;
                            intersectTile.IsUnitOrigin = true;
                        }
                    }
                }
            }
            else
            {
                //Single tile units have their anchor and origin in the same place.
                unit.OriginTile = tile;
                tile.IsUnitOrigin = true;
            }   
        }
    }
}
