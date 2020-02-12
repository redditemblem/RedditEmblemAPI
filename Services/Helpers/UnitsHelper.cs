using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
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
        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static Dictionary<string, Unit> Process(UnitsConfig config, IList<Item> items, IList<Skill> skills)
        {
            Dictionary<string, Unit> units = new Dictionary<string, Unit>();

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
                        TextFields = BuildTextFieldList(unit, config.TextFields),
                        Coordinates = new Coordinate(unit.ElementAtOrDefault(config.Coordinates) ?? string.Empty),
                        HP = new HP((unit.ElementAtOrDefault(config.CurrentHP) ?? string.Empty),
                                    (unit.ElementAtOrDefault(config.MaxHP) ?? string.Empty)),
                        Level = SafeIntParse(unit.ElementAtOrDefault(config.Level), "Level", true),
                        Class = unit.ElementAtOrDefault(config.Class) ?? string.Empty,
                        Affiliation = unit.ElementAtOrDefault(config.Affiliation) ?? string.Empty,
                        Experience = SafeIntParse(unit.ElementAtOrDefault(config.Experience) ?? string.Empty, "Experience", true) % 100,
                        Tags = BuildTagsList(unit.ElementAtOrDefault(config.Tags) ?? string.Empty),
                        Stats = BuildStatsDictionary(unit, config.Stats),
                        Inventory = BuildInventory(unit, config.Inventory, items),
                        Skills = BuildSkills(unit, config.Skills, skills)
                    };

                    //Apply stat boosts from items
                    //Equipped item
                    Item eqp = temp.Inventory.FirstOrDefault(i => i != null && i.IsEquipped);
                    if(eqp != null)
                    {
                        foreach(string stat in eqp.EquippedStatModifiers.Keys)
                        {
                            ModifiedStatValue mods = temp.Stats.GetValueOrDefault(stat);
                            if (mods != null)
                                mods.Modifiers.Add(string.Format("{0} ({1})", eqp.Name, "Eqp"), eqp.EquippedStatModifiers[stat]);
                        }
                    }
                    
                    //Inventory items
                    foreach(Item inv in temp.Inventory.Where(i => i != null && !i.IsEquipped))
                    {
                        foreach (string stat in inv.InventoryStatModifiers.Keys)
                        {
                            ModifiedStatValue mods = temp.Stats.GetValueOrDefault(stat);
                            if (mods != null)
                                mods.Modifiers.Add(string.Format("{0} ({1})", inv.Name, "Inv"), inv.InventoryStatModifiers[stat]);
                        }
                    }

                    units.Add(temp.Name, temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(row.ElementAtOrDefault(config.UnitName).ToString(), ex);
                }
            }

            return units;
        }

        private static IList<string> BuildTextFieldList(IList<string> unit, IList<int> configFields)
        {
            IList<string> fields = new List<string>();
            foreach (int field in configFields)
                if (!string.IsNullOrEmpty(unit.ElementAtOrDefault(field)))
                    fields.Add(unit.ElementAtOrDefault(field));
            return fields;
        }

        private static IList<string> BuildTagsList(string tagsCSV)
        {
            IList<string> tags = new List<string>();
            foreach (string tag in tagsCSV.Split(','))
                if(!string.IsNullOrEmpty(tag))
                    tags.Add(tag.Trim());
            return tags;
        }

        private static Dictionary<string, ModifiedStatValue> BuildStatsDictionary(IList<string> unit, IList<ModifiedNamedStatConfig> config)
        {
            Dictionary<string, ModifiedStatValue> stats = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig s in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse base value
                int val;
                if (!int.TryParse(unit.ElementAtOrDefault(s.BaseValue), out val) || val < 0)
                    throw new PositiveIntegerException(s.SourceName, unit.ElementAtOrDefault(s.BaseValue) ?? string.Empty);
                temp.BaseValue = val;

                //Parse modifiers list
                foreach (NamedStatConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(unit.ElementAtOrDefault(mod.Value), out val))
                        throw new AnyIntegerException(string.Format("{0} {1}", s.SourceName, mod.SourceName), unit.ElementAtOrDefault(mod.Value) ?? string.Empty);

                    if(val != 0)
                        temp.Modifiers.Add(mod.SourceName, val);
                }

                stats.Add(s.SourceName, temp);
            }

            return stats;
        }

        private static IList<Item> BuildInventory(IList<string> unit, InventoryConfig config, IList<Item> items)
        {
            IList<Item> inventory = new List<Item>();

            foreach (int slot in config.Slots)
            {
                string name = unit.ElementAtOrDefault(slot);
                if (string.IsNullOrEmpty(name))
                {
                    inventory.Add(null);
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
                    clone.OriginalName = unit.ElementAtOrDefault(slot);
                    clone.IsDroppable = isDroppable;
                    clone.Uses = uses;

                    inventory.Add(clone);
                }
                else throw new UnmatchedItemException(name);
            }

            //Find the equipped item and flag it
            Item equipped = inventory.FirstOrDefault(i => i.OriginalName == (unit.ElementAtOrDefault(config.EquippedItem) ?? string.Empty));
            equipped.IsEquipped = (equipped != null);

            return inventory;
        }

        private static IList<Skill> BuildSkills(IList<string> unit, SkillListConfig config, IList<Skill> skills)
        {
            IList<Skill> skillList = new List<Skill>();

            foreach (int slot in config.Slots)
            {
                string name = unit.ElementAtOrDefault(slot);
                if (string.IsNullOrEmpty(name))
                    continue;

                Skill skillMatch = skills.FirstOrDefault(i => i.Name == name);
                if (skillMatch != null) skillList.Add(skillMatch);
                else throw new UnmatchedSkillException(name);
            }

            return skillList;
        }
    }
}
