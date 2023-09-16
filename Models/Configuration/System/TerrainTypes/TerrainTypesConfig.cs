using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TerrainTypes"</c> object data.
    /// </summary>
    public class TerrainTypesConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index for a terrain type's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. List of containers for stat groups.
        /// </summary>
        [JsonRequired]
        public List<TerrainTypeStatsConfig> StatGroups { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a terrain type's warp type.
        /// </summary>
        public int WarpType { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a terrain type's warp cost.
        /// </summary>
        public int WarpCost { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for terrain type's cannot stop on flags.
        /// </summary>
        public int CannotStopOn { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a terrain type's blocks items flag.
        /// </summary>
        public int BlocksItems { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a terrain type's affiliation restrictions value.
        /// </summary>
        public int RestrictAffiliations { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a terrain type's groupings value.
        /// </summary>
        public int Groupings { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a terrain type's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}