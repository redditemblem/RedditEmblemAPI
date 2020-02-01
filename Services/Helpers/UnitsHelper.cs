using RedditEmblemAPI.Models;
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
    public static class UnitsHelper
    {
        private static Regex usesRegex = new Regex(@"\([0-9]+\)"); //match item uses (ex. "(5)")
        private static Regex dropRegex = new Regex(@"\(D\)");      //match item droppable (ex. "(D)")

        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Unit output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static IList<Unit> Process(IList<IList<object>> data, UnitsConfig config, IList<Item> items, IList<Skill> skills)
        {
            IList<Unit> units = new List<Unit>();

            foreach (IList<object> row in data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();

                    Unit temp = new Unit()
                    {
                        Name = unit.ElementAtOrDefault<string>(config.UnitName),
                        SpriteURL = unit.ElementAtOrDefault<string>(config.SpriteURL),
                        Coordinates = new Coordinate(unit.ElementAtOrDefault<string>(config.Coordinates)),
                        Stats = BuildStatsDictionary(unit, config.Stats),
                        Inventory = BuildInventory(unit, config.Inventory, items),
                        Skills = BuildSkills(unit, config.Skills, skills)
                    };

                    units.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(row.ElementAtOrDefault<object>(config.UnitName).ToString(), ex);
                }
            }

            return units;
        }

        private static Dictionary<string, ModifiedStatValue> BuildStatsDictionary(IList<string> unit, IList<ModifiedNamedStatConfig> config)
        {
            Dictionary<string, ModifiedStatValue> stats = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig s in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse base value
                int val;
                if (!int.TryParse(unit.ElementAtOrDefault<string>(s.BaseValue), out val))
                    throw new PositiveIntegerException("", unit.ElementAtOrDefault<string>(s.BaseValue));
                temp.BaseValue = val;

                //Parse modifiers list
                foreach (NamedStatConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(unit.ElementAtOrDefault<string>(mod.Value), out val))
                        throw new AnyIntegerException("", unit.ElementAtOrDefault<string>(mod.Value));
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                stats.Add(s.Name, temp);
            }

            return stats;
        }

        private static IList<Item> BuildInventory(IList<string> unit, InventoryConfig config, IList<Item> items)
        {
            IList<Item> inventory = new List<Item>();

            foreach (int slot in config.Slots)
            {
                string name = unit.ElementAtOrDefault<string>(slot);
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
                if(itemMatch != null)
                {
                    //If we find a match from the items sheet, deep clone it to the unit and paste in our values
                    Item clone = (Item)itemMatch.Clone();
                    clone.OriginalName = unit.ElementAtOrDefault<string>(slot);
                    clone.IsDroppable = isDroppable;
                    clone.Uses = uses;

                    inventory.Add(clone);
                }
            }

            //Find the equipped item and flag it
            Item equipped = inventory.FirstOrDefault(i => i.OriginalName == unit.ElementAtOrDefault<string>(config.EquippedItem));
            equipped.IsEquipped = (equipped != null);

            return inventory;
        }

        private static IList<Skill> BuildSkills(IList<string> unit, SkillListConfig config, IList<Skill> skills)
        {
            IList<Skill> skillList = new List<Skill>();

            foreach (int slot in config.Slots)
            {
                string name = unit.ElementAtOrDefault<string>(slot);
                if (string.IsNullOrEmpty(name))
                    continue;

                Skill skillMatch = skills.FirstOrDefault(i => i.Name == name);
                if (skillMatch != null)
                    skillList.Add(skillMatch);
            }

            return skillList;
        }
    }
}
