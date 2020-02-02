using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Items;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public static class ItemsHelper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Item output objects.
        /// </summary>
        /// <param name="data">Matrix of sheet Value values representing item data</param>
        /// <param name="config">Parsed JSON configuration mapping Values to output</param>
        /// <returns></returns>
        public static IList<Item> Process(IList<IList<object>> data, ItemsConfig config)
        {
            IList<Item> items = new List<Item>();

            foreach (IList<object> row in data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> item = row.Select(r => r.ToString()).ToList();

                    //Skip empty values
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault(config.ItemName)))
                        continue;

                    Item temp = new Item()
                    {
                        Name = item.ElementAtOrDefault(config.ItemName) ?? string.Empty,
                        SpriteURL = item.ElementAtOrDefault(config.SpriteURL) ?? string.Empty,
                        Category = item.ElementAtOrDefault(config.Category) ?? string.Empty,
                        WeaponRank = item.ElementAtOrDefault(config.WeaponRank) ?? string.Empty,
                        UtilizedStat = item.ElementAtOrDefault(config.UtilizedStat) ?? string.Empty,
                        MaxUses = int.Parse(item.ElementAtOrDefault(config.Uses)),
                        Stats = BuildStatsDictionary(item, config.Stats),
                        Range = BuildItemRange(item, config.Range) 
                    };

                    //Parse lists
                    foreach(NamedStatConfig stat in config.EquippedStatModifiers)
                    {
                        int val;
                        if(int.TryParse(item.ElementAtOrDefault(stat.Value), out val) && val != 0)
                            temp.EquippedStatModifiers.Add(stat.SourceName, val);
                    }

                    foreach (NamedStatConfig stat in config.InventoryStatModifiers)
                    {
                        int val;
                        if (int.TryParse(item.ElementAtOrDefault(stat.Value), out val) && val != 0)
                            temp.InventoryStatModifiers.Add(stat.SourceName, val);
                    }

                    foreach (int loc in config.TextFields)
                        if(!string.IsNullOrEmpty(item.ElementAtOrDefault(loc)))
                            temp.TextFields.Add(item.ElementAtOrDefault(loc));

                    items.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException(row.ElementAtOrDefault(config.ItemName).ToString(), ex);
                }
            }

            return items;
        }

        private static Dictionary<string, int> BuildStatsDictionary(IList<string> item, IList<NamedStatConfig> config)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig s in config)
            {
                //Parse value
                int val;
                if (!int.TryParse(item.ElementAtOrDefault(s.Value), out val))
                    throw new AnyIntegerException("", item.ElementAtOrDefault(s.Value) ?? string.Empty);
               
                stats.Add(s.SourceName, val);
            }

            return stats;
        }
    
        private static ItemRange BuildItemRange(IList<string> item, RangeConfig config)
        {
            ItemRange range = new ItemRange();

            //Parse minimum range value
            int val;
            if (!int.TryParse(item.ElementAtOrDefault(config.Minimum), out val) || val < 0)
                throw new PositiveIntegerException("", item.ElementAtOrDefault(config.Minimum) ?? string.Empty);
            range.Minimum = val;

            //Parse maximum range value
            if (!int.TryParse(item.ElementAtOrDefault(config.Maximum), out val) || val < 0)
                throw new PositiveIntegerException("", item.ElementAtOrDefault(config.Maximum) ?? string.Empty);
            range.Maximum = val;

            return range;
        }
    }
}
