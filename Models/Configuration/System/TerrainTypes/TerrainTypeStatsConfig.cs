using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"StatGroups"</c> object data.
    /// </summary>
    public class TerrainTypeStatsConfig
    {
        #region Required Attributes

        /// <summary>
        /// Required. List of movement costs for a terrain type.
        /// </summary>
        [JsonRequired]
        public List<NamedStatConfig> MovementCosts { get; set; }

        #endregion Required Attributes

        #region Optional Attributes

        /// <summary>
        /// Optional. Cell index for affiliation groupings for this terrain type stat set.
        /// </summary>
        public int AffiliationGroupings { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a terrain type's hit point modifier.
        /// </summary>
        public int HPModifier { get; set; } = -1;

        /// <summary>
        /// Optional. List of combat stat modifiers for a terrain type.
        /// </summary>
        public List<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of stat modifiers for a terrain type.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        #endregion Optional Attributes
    }
}
