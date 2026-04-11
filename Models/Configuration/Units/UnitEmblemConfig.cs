using Newtonsoft.Json;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitEmblemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of the emblem's name.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        /// <summary>
        /// Required. Location of the emblem's current engage meter count.
        /// </summary>
        [JsonRequired]
        public (int, int) EngageMeterCount { get; set; }

        /// <summary>
        /// Required. Location of the flag indicating whether or not the unit is engaged with this emblem.
        /// </summary>
        [JsonRequired]
        public (int, int) IsEngaged { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the unit's bond level with this emblem.
        /// </summary>
        public (int, int) BondLevel { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of container objects for the emblem's sync skills.
        /// </summary>
        public UnitSkillConfig[] SyncSkills { get; set; } = Array.Empty<UnitSkillConfig>();

        /// <summary>
        /// Optional. Collection of container objects for the emblem's engage skills.
        /// </summary>
        public UnitSkillConfig[] EngageSkills { get; set; } = Array.Empty<UnitSkillConfig>();

        /// <summary>
        /// Optional. Collection of locations of the emblem's engage weapons.
        /// </summary>
        public (int, int)[] EngageWeapons { get; set; } = Array.Empty<(int, int)>();

        /// <summary>
        /// Optional. Collection of locations of the emblem's engage attacks.
        /// </summary>
        public (int, int)[] EngageAttacks { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
