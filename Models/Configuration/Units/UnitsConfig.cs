using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
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
        /// List of cell indexes for unit's class name values.
        /// </summary>
        [JsonRequired]
        public IList<int> Classes { get; set; }

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
        /// List of equations to calculate unit combat stats.
        /// </summary>
        [JsonRequired]
        public IList<CalculatedStatConfig> CalculatedStats { get; set; }

        /// <summary>
        /// List of modified named stat configurations.
        /// </summary>
        [JsonRequired]
        public IList<ModifiedNamedStatConfig> Stats { get; set; }

        /// <summary>
        /// List of weapon ranks.
        /// </summary>
        [JsonRequired]
        public IList<UnitWeaponRanksConfig> WeaponRanks { get; set; }

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
        /// Cell index of the unit's held currency amount.
        /// </summary>
        public int HeldCurrency { get; set; } = -1;

        /// <summary>
        /// Cell index of a unit's movement flag value.
        /// </summary>
        public int HasMoved { get; set; } = -1;

        /// <summary>
        /// Cell index of a unit's tags value.
        /// </summary>
        public int Tags { get; set; } = -1;

        /// <summary>
        /// Cell index of the unit's size in number of tiles.
        /// </summary>
        public int UnitSize { get; set; } = -1;

        /// <summary>
        /// List of cell indexes for a unit's status conditions.
        /// </summary>
        public IList<int> Statuses { get; set; } = new List<int>();

        #endregion
    }
}