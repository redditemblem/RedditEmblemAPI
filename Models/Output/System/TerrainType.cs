using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="TerrainType"/>
    public interface ITerrainType : IMatchable
    {
        /// <inheritdoc cref="TerrainType.StatGroups"/>
        List<ITerrainTypeStats> StatGroups { get; set; }

        /// <inheritdoc cref="TerrainType.WarpType"/>
        WarpType WarpType { get; set; }

        /// <inheritdoc cref="TerrainType.WarpCost"/>
        int WarpCost { get; set; }

        /// <inheritdoc cref="TerrainType.CannotStopOn"/>
        bool CannotStopOn { get; set; }

        /// <inheritdoc cref="TerrainType.BlocksItems"/>
        bool BlocksItems { get; set; }

        /// <inheritdoc cref="TerrainType.RestrictAffiliations"/>
        List<int> RestrictAffiliations { get; set; }

        /// <inheritdoc cref="TerrainType.Groupings"/>
        List<int> Groupings { get; set; }

        /// <inheritdoc cref="TerrainType.TextFields"/>
        List<string> TextFields { get; set; }

        /// <inheritdoc cref="TerrainType.GetTerrainTypeStatsByAffiliation(IAffiliation)"/>
        ITerrainTypeStats GetTerrainTypeStatsByAffiliation(IAffiliation affiliation);

        /// <inheritdoc cref="TerrainType.GetTerrainTypeStatsByAffiliation(int)"/>
        ITerrainTypeStats GetTerrainTypeStatsByAffiliation(int affiliationGrouping);
    }

    #endregion Interface

    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType : Matchable, ITerrainType
    {
        #region Attributes

        /// <summary>
        /// List of stat groups.
        /// </summary>
        public List<ITerrainTypeStats> StatGroups { get; set; }

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
        public TerrainType(TerrainTypesConfig config, IEnumerable<string> data, IDictionary<string, IAffiliation> affiliations)
        {
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
        private List<ITerrainTypeStats> BuildTerrainTypeStats(IEnumerable<string> data, List<TerrainTypeStatsConfig> configs, IDictionary<string, IAffiliation> affiliations)
        {
            List<ITerrainTypeStats> groups = new List<ITerrainTypeStats>();
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
        public ITerrainTypeStats GetTerrainTypeStatsByAffiliation(IAffiliation affiliation)
        {
            return GetTerrainTypeStatsByAffiliation(affiliation.Grouping);
        }

        /// <summary>
        /// Returns the <c>TerrainTypeStats</c> that matches <paramref name="affiliationGrouping"/>. If one is not found, returns the default group instead.
        /// </summary>
        public ITerrainTypeStats GetTerrainTypeStatsByAffiliation(int affiliationGrouping)
        {
            ITerrainTypeStats match = this.StatGroups.FirstOrDefault(g => g.AffiliationGroupings.Contains(affiliationGrouping));
            if (match != null) return match;

            return this.StatGroups.First(g => g.IsDefaultGroup);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>ITerrainType</c> from each valid row.
        /// </summary>
        /// <exception cref="TerrainTypeProcessingException"></exception>
        public static IDictionary<string, ITerrainType> BuildDictionary(TerrainTypesConfig config, IDictionary<string, IAffiliation> affiliations)
        {
            IDictionary<string, ITerrainType> terrainTypes = new Dictionary<string, ITerrainType>();
            if (config?.Queries is null) return terrainTypes;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
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
        /// Matches each string in <paramref name="names"/> to an <c>ITerrainType</c> in <paramref name="terrainTypes"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<ITerrainType> MatchNames(IDictionary<string, ITerrainType> terrainTypes, IEnumerable<string> names, ICoordinate coord, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(terrainTypes, n, coord, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>ITerrainType</c> in <paramref name="terrainTypes"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedTileTerrainTypeException"></exception>
        public static ITerrainType MatchName(IDictionary<string, ITerrainType> terrainTypes, string name, ICoordinate coord, bool flagAsMatched = true)
        {
            ITerrainType match;
            if (!terrainTypes.TryGetValue(name, out match))
                throw new UnmatchedTileTerrainTypeException(coord, name);

            if (flagAsMatched) match.FlagAsMatched();

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