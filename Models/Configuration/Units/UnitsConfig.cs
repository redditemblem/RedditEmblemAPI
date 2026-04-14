using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Units"</c> object data.
    /// </summary>
    public class UnitsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a unit's sprite image URL value.
        /// </summary>
        [JsonRequired]
        public (int, int) SpriteURL { get; set; }

        /// <summary>
        /// Required. Location of a unit's level value.
        /// </summary>
        [JsonRequired]
        public (int, int) Level { get; set; }

        /// <summary>
        /// Required. Location of a unit's affiliation value.
        /// </summary>
        [JsonRequired]
        public (int, int) Affiliation { get; set; }

        /// <summary>
        /// Required. Location of a unit's coordinate value.
        /// </summary>
        [JsonRequired]
        public (int, int) Coordinate { get; set; }

        /// <summary>
        /// Required. Container object for a unit's HP configuration.
        /// </summary>
        [JsonRequired]
        public HPConfig HP { get; set; }

        /// <summary>
        /// Required. Collection of equations to calculate a unit's combat stats.
        /// </summary>
        [JsonRequired]
        public CalculatedStatConfig[] CombatStats { get; set; }

        /// <summary>
        /// Required. Collection of a unit's base stats.
        /// </summary>
        [JsonRequired]
        public ModifiedNamedStatConfig_Displayed[] Stats { get; set; }

        /// <summary>
        /// Required. Collection of a unit's weapon ranks.
        /// </summary>
        [JsonRequired]
        public UnitWeaponRanksConfig[] WeaponRanks { get; set; }

        /// <summary>
        /// Required. Container object for a unit's inventory configuration.
        /// </summary>
        [JsonRequired]
        public InventoryConfig Inventory { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the unit's controlling player.
        /// </summary>
        public (int, int) Player { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the unit's character application URL value.
        /// </summary>
        public (int, int) CharacterApplicationURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the unit's portrait image URL value.
        /// </summary>
        public (int, int) PortraitURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a unit's class values.
        /// </summary>
        public (int, int)[] Classes { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Location of a unit's movement type.
        /// </summary>
        public (int, int) MovementType { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a unit's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Location of a unit's experience value.
        /// </summary>
        public (int, int) Experience { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the unit's held currency amount.
        /// </summary>
        public (int, int) HeldCurrency { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the unit's size in number of tiles.
        /// </summary>
        public (int, int) UnitSize { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a unit's movement flag value.
        /// </summary>
        public (int, int) HasMoved { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a unit's tag(s).
        /// </summary>
        public (int, int)[] Tags { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Location of a unit's behavior description.
        /// </summary>
        public (int, int) Behavior { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of a unit's system stats.
        /// </summary>
        public ModifiedNamedStatConfig_Displayed[] SystemStats { get; set; } = Array.Empty<ModifiedNamedStatConfig_Displayed>();

        /// <summary>
        /// Optional. Collection of container objects for a unit's status conditions.
        /// </summary>
        public UnitStatusConditionConfig[] StatusConditions { get; set; } = Array.Empty<UnitStatusConditionConfig>();

        /// <summary>
        /// Optional. Collection of container objects for a unit's skill subsection configs.
        /// </summary>
        public UnitSkillSubsectionConfig[] SkillSubsections { get; set; } = Array.Empty<UnitSkillSubsectionConfig>();

        /// <summary>
        /// Optional. Collection of locations of a unit's combat art(s).
        /// </summary>
        public (int, int)[] CombatArts { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Container object for a unit's battalion configuration.
        /// </summary>
        public UnitBattalionConfig Battalion { get; set; } = null;

        /// <summary>
        /// Optional. Collection of locations of a unit's adjutant(s).
        /// </summary>
        public (int, int)[] Adjutants { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Container object for a unit's emblem configuration.
        /// </summary>
        public UnitEmblemConfig Emblem { get; set; } = null;

        #endregion Optional Fields
    }
}