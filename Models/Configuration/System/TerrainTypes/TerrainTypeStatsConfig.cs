using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"StatGroups"</c> object data.
    /// </summary>
    public class TerrainTypeStatsConfig
    {
        #region Required Attributes

        /// <summary>
        /// Required. Collection of movement costs for a terrain type.
        /// </summary>
        [JsonRequired]
        public NamedStatConfig[] MovementCosts { get; set; }

        #endregion Required Attributes

        #region Optional Attributes

        /// <summary>
        /// Optional. Location of affiliation groupings for this terrain type stat set.
        /// </summary>
        public (int, int) AffiliationGroupings { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's hit point modifier.
        /// </summary>
        public (int, int) HPModifier { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of combat stat modifiers for a terrain type.
        /// </summary>
        public NamedStatConfig[] CombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of stat modifiers for a terrain type.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        #endregion Optional Attributes
    }
}
