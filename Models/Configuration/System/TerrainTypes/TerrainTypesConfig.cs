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
        /// Optional. Cell index for a terrain type's hit point modifier.
        /// </summary>
        public int HPModifier { get; set; } = -1;

        /// <summary>
        /// Optional. List of combat stat modifiers for a terrain type.
        /// </summary>
        public IList<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of stat modifiers for a terrain type.
        /// </summary>
        public IList<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. Cell index of a terrain type's warp type.
        /// </summary>
        public int WarpType { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a terrain type's warp cost.
        /// </summary>
        public int WarpCost { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a terrain type's groupings value.
        /// </summary>
        public int Groupings { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a terrain type's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}