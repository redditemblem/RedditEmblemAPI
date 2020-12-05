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
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a unit's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of a unit's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public int SpriteURL { get; set; }

        /// <summary>
        /// Required. Cell index of a unit's level value.
        /// </summary>
        [JsonRequired]
        public int Level { get; set; }

        /// <summary>
        /// Required. Cell index of a unit's affiliation value.
        /// </summary>
        [JsonRequired]
        public int Affiliation { get; set; }

        /// <summary>
        /// Required. Cell index of a unit's coordinate value.
        /// </summary>
        [JsonRequired]
        public int Coordinate { get; set; }

        /// <summary>
        /// Required. Container object for a unit's HP configuration.
        /// </summary>
        [JsonRequired]
        public HPConfig HP { get; set; }

        /// <summary>
        /// Required. List of equations to calculate a unit's combat stats.
        /// </summary>
        [JsonRequired]
        public IList<CalculatedStatConfig> CombatStats { get; set; }

        /// <summary>
        /// Required. List of a unit's base stats.
        /// </summary>
        [JsonRequired]
        public IList<ModifiedNamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Required. List of a unit's weapon ranks.
        /// </summary>
        [JsonRequired]
        public IList<UnitWeaponRanksConfig> WeaponRanks { get; set; }

        /// <summary>
        /// Required. Container object for a unit's inventory configuration.
        /// </summary>
        [JsonRequired]
        public InventoryConfig Inventory { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of the unit's controlling player.
        /// </summary>
        public int Player { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a unit's class values.
        /// </summary>
        public IList<int> Classes { get; set; } = new List<int>();

        /// <summary>
        /// Optional. The cell index for a unit's movement type.
        /// </summary>
        public int MovementType { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a unit's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Cell index of a unit's experience value.
        /// </summary>
        public int Experience { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of the unit's held currency amount.
        /// </summary>
        public int HeldCurrency { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of the unit's size in number of tiles.
        /// </summary>
        public int UnitSize { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a unit's movement flag value.
        /// </summary>
        public int HasMoved { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a unit's tags value.
        /// </summary>
        public int Tags { get; set; } = -1;

        /// <summary>
        /// Optional. List of a unit's system stats.
        /// </summary>
        public IList<ModifiedNamedStatConfig> SystemStats { get; set; } = new List<ModifiedNamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for a unit's status conditions.
        /// </summary>
        public IList<int> StatusConditions { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Container object for a unit's skills configuration.
        /// </summary>
        public IList<int> Skills { get; set; } = new List<int>();

        #endregion
    }
}