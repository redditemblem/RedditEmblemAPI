using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public class TerrainTypeHelper : Helper
    {
        /// <summary>
        /// Parses Google Sheets data matrix to return a list of TerrainType output objects.
        /// </summary>
        /// <param name="config">Parsed JSON configuration.</param>
        /// <returns></returns>
        public static IDictionary<string, TerrainType> Process(TerrainTypesConfig config)
        {
            Dictionary<string, TerrainType> terrainTypes = new Dictionary<string, TerrainType>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> type = row.Select(r => r.ToString()).ToList();

                    //Skip blank terrain types
                    if (string.IsNullOrEmpty(type.ElementAtOrDefault(config.TypeName)))
                        continue;

                    TerrainType temp = new TerrainType()
                    {
                        Name = type.ElementAtOrDefault(config.TypeName).Trim(),
                        BlocksItems = ((type.ElementAtOrDefault(config.BlocksItems) ?? "No").Trim() == "Yes")
                    };

                    BuildMovementCostsDictionary(temp, type, config.MovementCosts);

                    terrainTypes.Add(temp.Name, temp);
                }
                catch (Exception ex)
                {
                    throw new TerrainTypeProcessingException(row.ElementAtOrDefault(config.TypeName).ToString(), ex);
                }
            }

            return terrainTypes;
        }

        private static void BuildMovementCostsDictionary(TerrainType type, IList<string> data, IList<NamedStatConfig> configs)
        {
            foreach(NamedStatConfig stat in configs)
            {
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(stat.Value), out val))
                    throw new AnyIntegerException(stat.SourceName, data.ElementAtOrDefault(stat.Value));
                type.MovementCosts.Add(stat.SourceName, val);
            }
        }
    }
}
