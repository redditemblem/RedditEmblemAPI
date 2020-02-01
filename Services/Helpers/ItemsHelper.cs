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
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(config.ItemName)))
                        continue;

                    Item temp = new Item()
                    {
                        Name = item.ElementAtOrDefault<string>(config.ItemName),
                        SpriteURL = item.ElementAtOrDefault<string>(config.SpriteURL),
                        Category = item.ElementAtOrDefault<string>(config.Category),
                        WeaponRank = item.ElementAtOrDefault<string>(config.WeaponRank),
                        UtilizedStat = item.ElementAtOrDefault<string>(config.UtilizedStat),
                        MaxUses = int.Parse(item.ElementAtOrDefault<string>(config.Uses)),
                        Stats = BuildStatsDictionary(item, config.Stats),
                        Range = BuildItemRange(item, config.Range)
                    };

                    foreach (int Value in config.TextFields)
                        temp.TextFields.Add(item.ElementAtOrDefault<string>(Value));

                    items.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException(row.ElementAtOrDefault<object>(config.ItemName).ToString(), ex);
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
                if (!int.TryParse(item.ElementAtOrDefault<string>(s.Value), out val))
                    throw new AnyIntegerException("", item.ElementAtOrDefault<string>(s.Value));
               
                stats.Add(s.SourceName, val);
            }

            return stats;
        }
    
        private static ItemRange BuildItemRange(IList<string> item, RangeConfig config)
        {
            ItemRange range = new ItemRange();

            //Parse minimum range value
            int val;
            if (!int.TryParse(item.ElementAtOrDefault<string>(config.Minimum), out val) || val < 0)
                throw new PositiveIntegerException("", item.ElementAtOrDefault<string>(config.Minimum));
            range.Minimum = val;

            //Parse maximum range value
            if (!int.TryParse(item.ElementAtOrDefault<string>(config.Maximum), out val) || val < 0)
                throw new PositiveIntegerException("", item.ElementAtOrDefault<string>(config.Maximum));
            range.Maximum = val;

            return range;
        }
    }
}
