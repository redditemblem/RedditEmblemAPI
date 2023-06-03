using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitSkillConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for the name of the skill.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. List of configs for any additional stats the skill may track.
        /// </summary>
        public List<NamedStatConfig> AdditionalStats { get; set; } = new List<NamedStatConfig>();

        #endregion
    }
}
