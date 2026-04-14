using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.CombatArts
{
    /// <summary>
    /// Container class for deserialized JSON <c>"CombatArts"</c> object data.
    /// </summary>
    public class CombatArtsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Container object for a combat art's range configuration.
        /// </summary>
        [JsonRequired]
        public CombatArtRangeConfig Range { get; set; }

        /// <summary>
        /// Required. Collection of a combat art's stats.
        /// </summary>
        [JsonRequired]
        public NamedStatConfig[] Stats { get; set; }

        /// <summary>
        /// Required. Location of the durability cost of a combat art.
        /// </summary>
        [JsonRequired]
        public (int, int) DurabilityCost { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of an combat art's sprite image URL value.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a combat art's weapon rank value.
        /// </summary>
        public (int, int) WeaponRank { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a combat art's category value.
        /// </summary>
        public (int, int) Category { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a combat art's utilized stat value(s).
        /// </summary>
        public (int, int)[] UtilizedStats { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of a combat art's tag(s).
        /// </summary>
        public (int, int)[] Tags { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of a combat art's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
