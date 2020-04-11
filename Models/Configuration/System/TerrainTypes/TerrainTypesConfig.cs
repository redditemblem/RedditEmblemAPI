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
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index for the name of the terrain type.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// List of cell indexes for terrain type movement costs.
        /// </summary>
        [JsonRequired]
        public IList<NamedStatConfig> MovementCosts { get; set; }

        /// <summary>
        /// Cell index for the blocks items flag.
        /// </summary>
        [JsonRequired]
        public int BlocksItems { get; set; }

        #endregion
    }
}