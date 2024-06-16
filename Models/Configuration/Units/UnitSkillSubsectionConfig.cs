using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitSkillSubsectionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. List of container objects for a unit's skills.
        /// </summary>
        [JsonRequired]
        public List<UnitSkillConfig> Skills { get; set; } = new List<UnitSkillConfig>();

        #endregion Required Fields
    }
}
