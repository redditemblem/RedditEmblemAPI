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
        public List<CalculatedStatConfig> CombatStats { get; set; }

        /// <summary>
        /// Required. List of a unit's base stats.
        /// </summary>
        [JsonRequired]
        public List<ModifiedNamedStatConfig> Stats { get; set; }

        /// <summary>
        /// Required. List of a unit's weapon ranks.
        /// </summary>
        [JsonRequired]
        public List<UnitWeaponRanksConfig> WeaponRanks { get; set; }

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
        /// Optional. Cell index of the unit's portrait image URL value.
        /// </summary>
        public int PortraitURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a unit's class values.
        /// </summary>
        public List<int> Classes { get; set; } = new List<int>();

        /// <summary>
        /// Optional. The cell index for a unit's movement type.
        /// </summary>
        public int MovementType { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a unit's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

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
        /// Optional. List of cell indexes for a unit's tag(s).
        /// </summary>
        public List<int> Tags { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Cell index of a unit's behavior description.
        /// </summary>
        public int Behavior { get; set; } = -1;

        /// <summary>
        /// Optional. List of a unit's system stats.
        /// </summary>
        public List<ModifiedNamedStatConfig> SystemStats { get; set; } = new List<ModifiedNamedStatConfig>();

        /// <summary>
        /// Optional. List of cell indexes for a unit's status conditions.
        /// </summary>
        public List<int> StatusConditions { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of cell indexes for a unit's skills.
        /// </summary>
        public List<int> Skills { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Container object for a unit's battalion configuration.
        /// </summary>
        public UnitBattalionConfig Battalion { get; set; } = null;

        #endregion
    }
}