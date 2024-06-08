using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TileObjects
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TileObjects"</c> object data.
    /// </summary>
    public class TileObjectsConfig : IMultiQueryable
    {
        #region Required Fields

        [JsonRequired]
        public IEnumerable<Query> Queries { get; set; }

        /// <summary>
        /// Required. Cell index for a tile object's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index for a tile object's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public int SpriteURL { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a tile object's size value.
        /// </summary>
        public int Size { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a tile object's layer value.
        /// </summary>
        public int Layer { get; set; } = -1;

        /// <summary>
        /// Optional. Container object for a tile object's range config.
        /// </summary>
        public TileObjectRangeConfig Range { get; set; } = null;

        /// <summary>
        /// Optional. Cell index for a tile object's hit point modifier value.
        /// </summary>
        public int HPModifier { get; set; } = -1;

        /// <summary>
        /// Optional. List of a tile object's combat stat modifiers.
        /// </summary>
        public List<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of a tile object's combat stat modifiers.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for a tile object's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
