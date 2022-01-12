using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this terrain type was found on a tile. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the terrain type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value by which the terrain effect modifies a unit's HP. Assumed to be a percentage.
        /// </summary>
        public int HPModifier { get; set; }

        /// <summary>
        /// List of combat stat modifiers applied by the terrain type.
        /// </summary>
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// List of stat modifiers applied by the terrain type.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of movement costs for the terrain type.
        /// </summary>
        public IDictionary<string, int> MovementCosts { get; set; }

        /// <summary>
        /// The warp type of the terrain effect.
        /// </summary>
        public WarpType WarpType { get; set; }

        /// <summary>
        /// The cost to begin a warp starting from this terrain effect, if applicable.
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
        public IList<int> RestrictAffiliations { get; set; }

        /// <summary>
        /// Only for JSON serialization. True when there's any items in the <c>RestrictAffiliations</c> list.
        /// </summary>
        [JsonProperty]
        private bool CanRestrictAffiliations { get { return this.RestrictAffiliations.Any(); } }

        /// <summary>
        /// The groupings that the terrain type belongs to.
        /// </summary>
        [JsonIgnore]
        public IList<int> Groupings { get; set; }

        /// <summary>
        /// List of text fields for the terrain type.
        /// </summary>
        public IList<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainType(TerrainTypesConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.CannotStopOn = (ParseHelper.SafeStringParse(data, config.CannotStopOn, "Cannot Stop On", false) == "Yes");
            this.BlocksItems = (ParseHelper.SafeStringParse(data, config.BlocksItems, "Blocks Items", false) == "Yes");
            this.RestrictAffiliations = ParseHelper.IntCSVParse(data, config.RestrictAffiliations, "Restrict Affiliations", true);
            this.Groupings = ParseHelper.IntCSVParse(data, config.Groupings, "Groupings", true);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.HPModifier = ParseHelper.OptionalInt_Any(data, config.HPModifier, "HP Modifier");

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = ParseHelper.OptionalInt_Any(data, stat.Value, stat.SourceName);
                if (val == 0) continue;
                this.CombatStatModifiers.Add(stat.SourceName, val);
            }


            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.StatModifiers)
            {
                int val = ParseHelper.OptionalInt_Any(data, stat.Value, stat.SourceName);
                if (val == 0) continue;
                this.StatModifiers.Add(stat.SourceName, val);
            }

            this.MovementCosts = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.MovementCosts)
            {
                int val = ParseHelper.Int_NonZeroPositive(data, stat.Value, $"{stat.SourceName} Movement Cost");
                this.MovementCosts.Add(stat.SourceName, val);
            }

            this.WarpType = GetWarpTypeEnum(ParseHelper.SafeStringParse(data, config.WarpType, "Warp Type", false));
            if (this.WarpType == WarpType.Entrance || this.WarpType == WarpType.Dual)
            {
                this.WarpCost = ParseHelper.Int_Positive(data, config.WarpCost, "Warp Cost");
            }
            else this.WarpCost = 0;
        }

        /// <summary>
        /// Converts the string value of <paramref name="warpType"/> into the corresponding <c>WarpType</c> object.
        /// </summary>
        /// <param name="warpType"></param>
        /// <exception cref="UnmatchedWarpTypeException"></exception>
        /// <returns></returns>
        private WarpType GetWarpTypeEnum(string warpType)
        {
            if (string.IsNullOrEmpty(warpType))
                return WarpType.None;

            object warpEnum;
            if (!Enum.TryParse(typeof(WarpType), warpType, out warpEnum))
                throw new UnmatchedWarpTypeException(warpType);

            return (WarpType)warpEnum;
        }

        #region Static Functions

        public static IDictionary<string, TerrainType> BuildDictionary(TerrainTypesConfig config)
        {
            IDictionary<string, TerrainType> terrainTypes = new Dictionary<string, TerrainType>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> type = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(type, config.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!terrainTypes.TryAdd(name, new TerrainType(config, type)))
                        throw new NonUniqueObjectNameException("terrain type");
                }
                catch (Exception ex)
                {
                    throw new TerrainTypeProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return terrainTypes;
        }

        #endregion
    }

    public enum WarpType
    {
        None = 0,
        Entrance = 1,
        Exit = 2,
        Dual = 3
    }
}