using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitEmblemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of the emblem's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of the emblem's current engage meter count.
        /// </summary>
        [JsonRequired]
        public int EngageMeterCount { get; set; }

        /// <summary>
        /// Required. Cell index of the flag indicating whether or not the unit is engaged with this emblem.
        /// </summary>
        [JsonRequired]
        public int IsEngaged { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of the unit's bond level with this emblem.
        /// </summary>
        public int BondLevel { get; set; } = -1;

        /// <summary>
        /// Optional. List of container objects for the emblem's sync skills.
        /// </summary>
        public List<UnitSkillConfig> SyncSkills { get; set; } = new List<UnitSkillConfig>();

        /// <summary>
        /// Optional. List of container objects for the emblem's engage skills.
        /// </summary>
        public List<UnitSkillConfig> EngageSkills { get; set; } = new List<UnitSkillConfig>();

        /// <summary>
        /// Optional. List of cell indexes for the emblem's engage weapons.
        /// </summary>
        public List<int> EngageWeapons { get; set; } = new List<int>();

        #endregion Optional Fields
    }
}
