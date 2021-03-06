﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.TerrainEffects
{
    /// <summary>
    /// Container class for deserialized JSON <c>"TerrainEffects"</c> object data.
    /// </summary>
    public class TerrainEffectsConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index for a terrain effect's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index for a terrain effect's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public int SpriteURL { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for a terrain effect's size value.
        /// </summary>
        public int Size { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for a terrain effect's hit point modifier value.
        /// </summary>
        public int HPModifier { get; set; } = -1;

        /// <summary>
        /// Optional. List of a terrain effect's combat stat modifiers.
        /// </summary>
        public IList<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of a terrain effect's combat stat modifiers.
        /// </summary>
        public IList<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for a terrain effect's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
