using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.TileObjects
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TileObjects"</c> object data.
    /// </summary>
    public class TileObjectsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a tile object's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public (int, int) SpriteURL { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Location of a tile object's size value.
        /// </summary>
        public (int, int) Size { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a tile object's layer value.
        /// </summary>
        public (int, int) Layer { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Container object for a tile object's range config.
        /// </summary>
        public TileObjectRangeConfig Range { get; set; } = null;

        /// <summary>
        /// Optional. Location of a tile object's hit point modifier value.
        /// </summary>
        public (int, int) HPModifier { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of a tile object's combat stat modifiers.
        /// </summary>
        public NamedStatConfig[] CombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of a tile object's combat stat modifiers.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of locations of a tile object's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
