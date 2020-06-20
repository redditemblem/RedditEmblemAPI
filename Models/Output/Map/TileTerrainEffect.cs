using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using System.Collections;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Object representing a <c>TerrainEffect</c> instance located on a <c>Tile</c>.
    /// </summary>
    public class TileTerrainEffect
    {
        /// <summary>
        /// Only for JSON serialization. The name of the terrain effect.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.TerrainEffect.Name;  } }

        /// <summary>
        /// The <c>TerrainEffect</c> located on the tile. 
        /// </summary>
        [JsonIgnore]
        public TerrainEffect TerrainEffect { get; set; }

        /// <summary>
        /// Flag indicating if the terrain effect is anchored here. Terrain effects will be drawn at the anchor tile.
        /// </summary>
        public bool IsAnchor { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TileTerrainEffect(TerrainEffect terrainEffect, bool isAnchor)
        {
            this.TerrainEffect = terrainEffect;
            this.IsAnchor = isAnchor;
        }
    }
}
