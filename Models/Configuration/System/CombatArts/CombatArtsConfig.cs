using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.CombatArts
{
    /// <summary>
    /// Container class for deserialized JSON <c>"CombatArts"</c> object data.
    /// </summary>
    public class CombatArtsConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a combat art's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Container object for a combat art's range configuration.
        /// </summary>
        [JsonRequired]
        public CombatArtRangeConfig Range { get; set; }

        /// <summary>
        /// Required. List of a combat art's stats.
        /// </summary>
        [JsonRequired]
        public List<NamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Required. Cell index of the durability cost of a combat art.
        /// </summary>
        [JsonRequired]
        public int DurabilityCost { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an combat art's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a combat art's weapon rank value.
        /// </summary>
        public int WeaponRank { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a combat art's category value.
        /// </summary>
        public int Category { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a combat art's utilized stat value.
        /// </summary>
        public List<int> UtilizedStats { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for a combat art's tag(s).
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for a combat art's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}
