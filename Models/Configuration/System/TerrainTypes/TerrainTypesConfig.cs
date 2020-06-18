using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainTypes
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TerrainTypes"</c> object data.
    /// </summary>
    public class TerrainTypesConfig
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
        /// Required. List of movement costs for a terrain type.
        /// </summary>
        [JsonRequired]
        public IList<NamedStatConfig> MovementCosts { get; set; }

        /// <summary>
        /// Required. Cell index for a terrain type's blocks items flag.
        /// </summary>
        [JsonRequired]
        public int BlocksItems { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. List of stat modifiers for a terrain type.
        /// </summary>
        public IList<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. Cell index of a terrain type's grouping value.
        /// </summary>
        public int Grouping { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a terrain type's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}