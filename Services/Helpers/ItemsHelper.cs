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
        /// <param name="data">Matrix of sheet cell values representing item data</param>
        /// <param name="config">Parsed JSON configuration mapping cells to output</param>
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

                    Item temp = new Item()
                    {
                        Name = item[config.ItemName],
                        SpriteURL = item[config.SpriteURL],
                        Category = item[config.Category],
                        WeaponRank = item[config.WeaponRank],
                        UtilizedStat = item[config.UtilizedStat],
                        MaxUses = int.Parse(item[config.Uses]),
                        Stats = BuildStatsDictionary(item, config.Stats),
                        Range = BuildItemRange(item, config.Range)
                    };

                    foreach (int cell in config.TextFields)
                        temp.TextFields.Add(item[cell]);

                    items.Add(temp);
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException(row[config.ItemName].ToString(), ex);
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
                if (!int.TryParse(item[s.Cell], out val))
                    throw new AnyIntegerException("", item[s.Cell]);
               
                stats.Add(s.SourceName, val);
            }

            return stats;
        }
    
        private static ItemRange BuildItemRange(IList<string> item, RangeConfig config)
        {
            ItemRange range = new ItemRange();

            //Parse minimum range value
            int val;
            if (!int.TryParse(item[config.Minimum], out val) || val < 0)
                throw new PositiveIntegerException("", item[config.Minimum]);
            range.Minimum = val;

            //Parse maximum range value
            if (!int.TryParse(item[config.Maximum], out val) || val < 0)
                throw new PositiveIntegerException("", item[config.Maximum]);
            range.Maximum = val;

            return range;
        }
    }
}
