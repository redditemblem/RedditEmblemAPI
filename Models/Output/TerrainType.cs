using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a TerrainType definition in the team's system.
    /// </summary>
    public class TerrainType
    {
        public TerrainType()
        {
            this.Matched = false;
            this.BlocksItems = false;

            this.MovementCosts = new Dictionary<string, int>();
        }

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
        /// List of movement costs for the terrain type.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> MovementCosts { get; set; }

        /// <summary>
        /// Flag indicating whether or not item ranges can pass through the terrain type.
        /// </summary>
        [JsonIgnore]
        public bool BlocksItems { get; set; }
    }
}