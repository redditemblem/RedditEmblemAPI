using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this terrain type was found on a tile. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The name of the terrain type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of stat groups.
        /// </summary>
        public List<TerrainTypeStats> StatGroups { get; set; }

        /// <summary>
        /// The warp type of the terrain type.
        /// </summary>
        public WarpType WarpType { get; set; }

        /// <summary>
        /// The cost to begin a warp starting from this terrain type, if applicable.
        /// </summary>
        public int WarpCost { get; set; }

        /// <summary>
        /// Flag indicating whether or not units are allowed to end their unit range on this tile.
        /// </summary>
        public bool CannotStopOn { get; set; }

        /// <summary>
        /// Flag indicating whether or not item ranges can pass through the terrain type.
        /// </summary>
        public bool BlocksItems { get; set; }

        /// <summary>
        /// List of affiliations that are capable of passing through the terrain type, if any.
        /// </summary>
        [JsonIgnore]
        public List<int> RestrictAffiliations { get; set; }

        /// <summary>
        /// The groupings that the terrain type belongs to.
        /// </summary>
        [JsonIgnore]
        public List<int> Groupings { get; set; }

        /// <summary>
        /// List of text fields for the terrain type.
        /// </summary>
        public List<string> TextFields { get; set; }

        #region JSON Serialization

        /// <summary>
        /// Only for JSON serialization. True when there's any items in the <c>RestrictAffiliations</c> list.
        /// </summary>
        [JsonProperty]
        private bool CanRestrictAffiliations { get { return this.RestrictAffiliations.Any(); } }

        #endregion JSON Serialization

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainType(TerrainTypesConfig config, IEnumerable<string> data, IDictionary<string, Affiliation> affiliations)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.CannotStopOn = DataParser.OptionalBoolean_YesNo(data, config.CannotStopOn, "Cannot Stop On");
            this.BlocksItems = DataParser.OptionalBoolean_YesNo(data, config.BlocksItems, "Blocks Items");
            this.RestrictAffiliations = DataParser.List_IntCSV(data, config.RestrictAffiliations, "Restrict Affiliations", true);
            this.Groupings = DataParser.List_IntCSV(data, config.Groupings, "Groupings", true);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
            this.StatGroups = BuildTerrainTypeStats(data, config.StatGroups, affiliations);

            this.WarpType = GetWarpTypeEnum(DataParser.OptionalString(data, config.WarpType, "Warp Type"));
            if (this.WarpType == WarpType.Entrance || this.WarpType == WarpType.Dual)
            {
                this.WarpCost = DataParser.Int_Positive(data, config.WarpCost, "Warp Cost");
            }
            else this.WarpCost = -1;
        }

        /// <summary>
        /// Iterates through <paramref name="configs"/> and builds a list of <c>TerrainTypeStats</c>.
        /// </summary>
        /// <exception cref="DuplicateTerrainTypeStatsException"></exception>
        private List<TerrainTypeStats> BuildTerrainTypeStats(IEnumerable<string> data, List<TerrainTypeStatsConfig> configs, IDictionary<string, Affiliation> affiliations)
        {
            List<TerrainTypeStats> groups = new List<TerrainTypeStats>();
            foreach (TerrainTypeStatsConfig config in configs)
            {
                //We want to include the group only if it's the default group or if it has at least one grouping value included.
                string values = DataParser.OptionalString(data, config.AffiliationGroupings, "Affiliation Groupings");
                if (config.AffiliationGroupings != -1 && string.IsNullOrEmpty(values))
                    continue;

                groups.Add(new TerrainTypeStats(config, data, affiliations));
            }

            //Check if an affiliation grouping is repeated anywhere. Each grouping can only be in one set.
            IEnumerable<IGrouping<int, int>> duplicates = groups.SelectMany(g => g.AffiliationGroupings).GroupBy(g => g).Where(g => g.Count() > 1);
            if (duplicates.Any())
                throw new DuplicateTerrainTypeStatsException(duplicates.First().Key);

            return groups;
        }

        /// <summary>
        /// Converts the string value of <paramref name="warpType"/> into the corresponding <c>WarpType</c> object.
        /// </summary>
        /// <exception cref="UnmatchedWarpTypeException"></exception>
        private WarpType GetWarpTypeEnum(string warpType)
        {
            if (string.IsNullOrEmpty(warpType))
                return WarpType.None;

            object warpEnum;
            if (!Enum.TryParse(typeof(WarpType), warpType, out warpEnum))
                throw new UnmatchedWarpTypeException(warpType);

            return (WarpType)warpEnum;
        }

        /// <summary>
        /// Returns the <c>TerrainTypeStats</c> that matches <paramref name="affiliation"/>. If one is not found, returns the default group instead.
        /// </summary>
        public TerrainTypeStats GetTerrainTypeStatsByAffiliation(Affiliation affiliation)
        {
            return GetTerrainTypeStatsByAffiliation(affiliation.Grouping);
        }

        /// <summary>
        /// Returns the <c>TerrainTypeStats</c> that matches <paramref name="affiliationGrouping"/>. If one is not found, returns the default group instead.
        /// </summary>
        public TerrainTypeStats GetTerrainTypeStatsByAffiliation(int affiliationGrouping)
        {
            TerrainTypeStats match = this.StatGroups.FirstOrDefault(g => g.AffiliationGroupings.Contains(affiliationGrouping));
            if (match != null) return match;

            return this.StatGroups.First(g => g.IsDefaultGroup);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>TerrainType</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>TerrainType</c> from each valid row.
        /// </summary>
        /// <exception cref="TerrainTypeProcessingException"></exception>
        public static IDictionary<string, TerrainType> BuildDictionary(TerrainTypesConfig config, IDictionary<string, Affiliation> affiliations)
        {
            IDictionary<string, TerrainType> terrainTypes = new Dictionary<string, TerrainType>();
            if (config == null || config.Query == null)
                return terrainTypes;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> type = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(type, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!terrainTypes.TryAdd(name, new TerrainType(config, type, affiliations)))
                        throw new NonUniqueObjectNameException("terrain type");
                }
                catch (Exception ex)
                {
                    throw new TerrainTypeProcessingException(name, ex);
                }
            }

            return terrainTypes;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>TerrainType</c> in <paramref name="terrainTypes"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<TerrainType> MatchNames(IDictionary<string, TerrainType> terrainTypes, IEnumerable<string> names, Coordinate coord, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(terrainTypes, n, coord, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>TerrainType</c> in <paramref name="terrainTypes"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedTileTerrainTypeException"></exception>
        public static TerrainType MatchName(IDictionary<string, TerrainType> terrainTypes, string name, Coordinate coord, bool skipMatchedStatusSet = false)
        {
            TerrainType match;
            if (!terrainTypes.TryGetValue(name, out match))
                throw new UnmatchedTileTerrainTypeException(coord, name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }

    public enum WarpType
    {
        None = 0,
        Entrance = 1,
        Exit = 2,
        Dual = 3
    }
}