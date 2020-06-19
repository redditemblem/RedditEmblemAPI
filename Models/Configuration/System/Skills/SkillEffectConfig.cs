using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Effect"</c> object data.
    /// </summary>
    public class SkillEffectConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of a skill effect's type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        /// <summary>
        /// Required. List of cell indexes for the parameters.
        /// </summary>
        [JsonRequired]
        public IList<int> Parameters { get; set; }

        #endregion
    }
}
