using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainEffects
{
    public class TerrainEffectsConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index for the name of the terrain effect.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index for the sprite URL of the terrain effect.
        /// </summary>
        [JsonRequired]
        public int SpriteURL { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Cell index for the size of the terrain effect.
        /// </summary>
        public int Size { get; set; } = -1;

        /// <summary>
        /// List of cell indexes for text information about the terrain effect.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
