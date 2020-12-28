using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType
    {
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
        /// Flag indicating whether or not item ranges can pass through the terrain type.
        /// </summary>
        [JsonIgnore]
        public bool BlocksItems { get; set; }

        /// <summary>
        /// The groupings that the terrain type belongs to.
        /// </summary>
        [JsonIgnore]
        public IList<int> Groupings { get; set; }

        /// <summary>
        /// List of text fields for the terrain type.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainType(TerrainTypesConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.BlocksItems = (ParseHelper.SafeStringParse(data, config.BlocksItems, "Blocks Items", true) == "Yes");
            this.Groupings = ParseHelper.IntCSVParse(data, config.Groupings, "Groupings", true);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);

            this.HPModifier = ParseHelper.OptionalSafeIntParse(data, config.HPModifier, "HP Modifier", false, false, 0);

            this.CombatStatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.CombatStatModifiers)
            {
                int val = ParseHelper.OptionalSafeIntParse(data, stat.Value, stat.SourceName, false, false, 0);
                if (val == 0) continue;
                this.CombatStatModifiers.Add(stat.SourceName, val);
            }


            this.StatModifiers = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.StatModifiers)
            {
                int val = ParseHelper.OptionalSafeIntParse(data, stat.Value, stat.SourceName, false, false, 0);
                if (val == 0) continue;
                this.StatModifiers.Add(stat.SourceName, val);
            }
            
            this.MovementCosts = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in config.MovementCosts)
            {
                int val = ParseHelper.SafeIntParse(data, stat.Value, string.Format("{0} Movement Cost", stat.SourceName), true, true);
                this.MovementCosts.Add(stat.SourceName, val);
            }

            this.WarpType = GetWarpTypeEnum(ParseHelper.SafeStringParse(data, config.WarpType, "Warp Type", false));
            if (this.WarpType == WarpType.Entrance || this.WarpType == WarpType.Dual)
            {
                this.WarpCost = ParseHelper.SafeIntParse(data, config.WarpCost, "Warp Cost", true);
            }
            else this.WarpCost = 0;
        }

        /// <summary>
        /// Converts the string value of <paramref name="warpTypeName"/> into the corresponding <c>WarpType</c> object.
        /// </summary>
        /// <param name="warpTypeName"></param>
        /// <exception cref="UnmatchedWarpTypeException"></exception>
        /// <returns></returns>
        private WarpType GetWarpTypeEnum(string warpTypeName)
        {
            switch (warpTypeName)
            {
                case "": return WarpType.None;
                case "Entrance": return WarpType.Entrance;
                case "Exit": return WarpType.Exit;
                case "Dual": return WarpType.Dual;
                default: throw new UnmatchedWarpTypeException(warpTypeName);
            }
        }
    }

    public enum WarpType
    {
        None = 0,
        Entrance = 1,
        Exit = 2,
        Dual = 3
    }
}