using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Effect"</c> object data.
    /// </summary>
    public class SkillEffectConfig
    {
        #region RequiredFields

        /// <summary>
        /// Required. Cell index of a skill effect's type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        /// <summary>
        /// Required. Cell index of the skill effect's first parameter.
        /// </summary>
        [JsonRequired]
        public int Parameter1 { get; set; }

        /// <summary>
        /// Required. Cell index of the skill effect's second parameter.
        /// </summary>
        [JsonRequired]
        public int Parameter2 { get; set; }

        #endregion
    }
}
