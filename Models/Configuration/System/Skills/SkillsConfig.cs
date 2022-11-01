using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Skills"</c> object data.
    /// </summary>
    public class SkillsConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a skill's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a skill's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for a skill's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        /// <summary>
        /// Optional. List of skill effect configurations.
        /// </summary>
        public List<SkillEffectConfig> Effects { get; set; } = new List<SkillEffectConfig>();

        #endregion
    }
}
