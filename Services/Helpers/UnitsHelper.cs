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
        /// <param name="data">Matrix of sheet cell values representing unit data</param>
        /// <param name="config">Parsed JSON configuration mapping cells to output</param>
        /// <returns></returns>
        public static IList<Unit> Process(IList<IList<object>> data, UnitsConfig config, IList<Item> items)
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
                        Name = unit[config.UnitName],
                        SpriteURL = unit[config.SpriteURL],
                        Coordinates = new Coordinate(unit[config.Coordinates]),
                        Stats = BuildStatsDictionary(unit, config.Stats),
                        Inventory = BuildInventory(unit, config.Inventory, items)
                    };

                    units.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(row[config.UnitName].ToString(), ex);
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
                if (!int.TryParse(unit[s.BaseValue], out val))
                    throw new PositiveIntegerException("", unit[s.BaseValue]);
                temp.BaseValue = val;

                //Parse modifiers list
                foreach (NamedStatConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(unit[mod.Cell], out val))
                        throw new AnyIntegerException("", unit[mod.Cell]);
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
                string name = unit[slot];
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
                    clone.OriginalName = unit[slot];
                    clone.IsDroppable = isDroppable;
                    clone.Uses = uses;

                    inventory.Add(clone);
                }
            }

            //Find the equipped item and flag it
            if (!string.IsNullOrEmpty(unit[config.EquippedItem]))
            {
                Item equipped = inventory.FirstOrDefault(i => i.OriginalName == unit[config.EquippedItem]);
                equipped.IsEquipped = (equipped != null);
            }

            return inventory;
        }
    }
}
