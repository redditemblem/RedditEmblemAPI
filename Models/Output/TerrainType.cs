using Newtonsoft.Json;

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
    }
}