using System;
using System.Text.Json.Serialization;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitSkillSubsectionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Collection of container objects for a unit's skills.
        /// </summary>
        [JsonRequired]
        public UnitSkillConfig[] Skills { get; set; } = Array.Empty<UnitSkillConfig>();

        #endregion Required Fields
    }
}
