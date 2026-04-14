using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitSkillConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of the name of the skill.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Collection of configs for any additional stats the skill may track.
        /// </summary>
        public NamedStatConfig[] AdditionalStats { get; set; } = Array.Empty<NamedStatConfig>();

        #endregion Optional Fields
    }
}
