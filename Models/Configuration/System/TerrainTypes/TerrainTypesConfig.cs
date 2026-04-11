using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TerrainTypes"</c> object data.
    /// </summary>
    public class TerrainTypesConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Collection of containers for stat groups.
        /// </summary>
        [JsonRequired]
        public TerrainTypeStatsConfig[] StatGroups { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of a terrain type's warp type.
        /// </summary>
        public (int, int) WarpType { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's warp cost.
        /// </summary>
        public (int, int) WarpCost { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's cannot stop on flags.
        /// </summary>
        public (int, int) CannotStopOn { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's blocks items flag.
        /// </summary>
        public (int, int) BlocksItems { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's affiliation restrictions value.
        /// </summary>
        public (int, int) RestrictAffiliations { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a terrain type's groupings value.
        /// </summary>
        public (int, int) Groupings { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a terrain type's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}