﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Units"</c> object data.
    /// </summary>
    public class UnitsConfig : IMultiQueryable
    {
        #region Required Fields

        [JsonRequired]
        public IEnumerable<Query> Queries { get; set; }

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
        public List<ModifiedNamedStatConfig_Displayed> Stats { get; set; }

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
        /// Optional. Cell index of the unit's character application URL value.
        /// </summary>
        public int CharacterApplicationURL { get; set; } = -1;

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
        public List<ModifiedNamedStatConfig_Displayed> SystemStats { get; set; } = new List<ModifiedNamedStatConfig_Displayed>();

        /// <summary>
        /// Optional. List of container objects for a unit's status conditions.
        /// </summary>
        public List<UnitStatusConditionConfig> StatusConditions { get; set; } = new List<UnitStatusConditionConfig>();

        /// <summary>
        /// Optional. List of container objects for a unit's skill subsection configs.
        /// </summary>
        public List<UnitSkillSubsectionConfig> SkillSubsections { get; set; } = new List<UnitSkillSubsectionConfig>();

        /// <summary>
        /// Optional. List of cell indexes of a unit's combat art(s).
        /// </summary>
        public List<int> CombatArts { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Container object for a unit's battalion configuration.
        /// </summary>
        public UnitBattalionConfig Battalion { get; set; } = null;

        /// <summary>
        /// Optional. List of cell indexes for a unit's adjutant(s).
        /// </summary>
        public List<int> Adjutants { get; set; } = new List<int>();

        /// <summary>
        /// Optional. Container object for a unit's emblem configuration.
        /// </summary>
        public UnitEmblemConfig Emblem { get; set; } = null;

        #endregion
    }
}