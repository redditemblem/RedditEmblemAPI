using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class ItemsHelper : Helper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of Item output objects.
        /// </summary>
        /// <param name="config">Parsed JSON configuration.</param>
        /// <returns></returns>
        public static IList<Item> Process(ItemsConfig config)
        {
            IList<Item> items = new List<Item>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> item = row.Select(r => r.ToString()).ToList();

                    //Skip blank items
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault(config.ItemName)))
                        continue;

                    Item temp = new Item()
                    {
                        Name = item.ElementAtOrDefault(config.ItemName).Trim(),
                        SpriteURL = (item.ElementAtOrDefault(config.SpriteURL) ?? string.Empty).Trim(),
                        Category = item.ElementAtOrDefault(config.Category) ?? string.Empty,
                        WeaponRank = item.ElementAtOrDefault(config.WeaponRank) ?? string.Empty,
                        UtilizedStat = item.ElementAtOrDefault(config.UtilizedStat) ?? string.Empty,
                        DealsDamage = ((item.ElementAtOrDefault(config.DealsDamage) ?? "No").Trim() == "Yes"),
                        MaxUses = SafeIntParse(item.ElementAtOrDefault(config.Uses), "Uses", true),
                        Stats = BuildStatsDictionary(item, config.Stats),
                        Range = new ItemRange((item.ElementAtOrDefault(config.Range.Minimum) ?? string.Empty),
                                              (item.ElementAtOrDefault(config.Range.Maximum) ?? string.Empty))
                    };

                    //Parse lists
                    //Equipped stat modifiers
                    foreach(NamedStatConfig stat in config.EquippedStatModifiers)
                    {
                        int val;
                        if (!int.TryParse(item.ElementAtOrDefault(stat.Value), out val))
                            throw new AnyIntegerException(string.Format("{0} ({1})", stat.SourceName, "Equipped"), item.ElementAtOrDefault(stat.Value) ?? string.Empty);

                        if (val != 0)
                            temp.EquippedStatModifiers.Add(stat.SourceName, val);
                    }

                    //Inventory stat modifiers
                    foreach (NamedStatConfig stat in config.InventoryStatModifiers)
                    {
                        int val;
                        if (!int.TryParse(item.ElementAtOrDefault(stat.Value), out val))
                            throw new AnyIntegerException(string.Format("{0} ({1})", stat.SourceName, "Inventory"), item.ElementAtOrDefault(stat.Value) ?? string.Empty);

                        if (val != 0)
                            temp.InventoryStatModifiers.Add(stat.SourceName, val);
                    }

                    //Flavor text fields
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
                    throw new AnyIntegerException(s.SourceName, item.ElementAtOrDefault(s.Value) ?? string.Empty);
               
                stats.Add(s.SourceName, val);
            }

            return stats;
        }
    }
}
