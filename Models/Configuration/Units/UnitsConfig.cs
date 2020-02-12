using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Units"</c> object data.
    /// </summary>
    public class UnitsConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index of a unit's name value.
        /// </summary>
        [JsonRequired]
        public int UnitName { get; set; }

        /// <summary>
        /// Cell index of a unit's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public int SpriteURL { get; set; }

        /// <summary>
        /// Cell index of a unit's coordinate value.
        /// </summary>
        [JsonRequired]
        public int Coordinates { get; set; }

        /// <summary>
        /// Cell index of a unit's level value.
        /// </summary>
        [JsonRequired]
        public int Level { get; set; }

        /// <summary>
        /// Cell index of a unit's class name value.
        /// </summary>
        [JsonRequired]
        public int Class { get; set; }

        /// <summary>
        /// Cell index of a unit's affiliation value.
        /// </summary>
        [JsonRequired]
        public int Affiliation { get; set; }

        /// <summary>
        /// Cell index of a unit's experience value.
        /// </summary>
        [JsonRequired]
        public int Experience { get; set; }

        /// <summary>
        /// Cell index of a unit's current HP value.
        /// </summary>
        [JsonRequired]
        public int CurrentHP { get; set; }

        /// <summary>
        /// Cell index of a unit's maximum HP value.
        /// </summary>
        [JsonRequired]
        public int MaxHP { get; set; }

        /// <summary>
        /// List of modified named stat configurations.
        /// </summary>
        [JsonRequired]
        public IList<ModifiedNamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Container object for a unit's inventory configuration.
        /// </summary>
        [JsonRequired]
        public InventoryConfig Inventory { get; set; }

        /// <summary>
        /// Container object for a unit's skills configuration.
        /// </summary>
        [JsonRequired]
        public SkillListConfig Skills { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// List of cell indexes for a unit's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        /// <summary>
        /// Cell index of a unit's movement flag value.
        /// </summary>
        public int HasMoved { get; set; } = -1;

        /// <summary>
        /// Cell index of a unit's tags value.
        /// </summary>
        public int Tags { get; set; } = -1;

        #endregion
    }
}