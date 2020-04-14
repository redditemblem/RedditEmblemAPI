using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    public class SkillEffectConfig
    {
        #region RequiredFields

        /// <summary>
        /// Cell index of a skill effects's type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        /// <summary>
        /// Cell index of the skill effect's first parameter.
        /// </summary>
        [JsonRequired]
        public int Parameter1 { get; set; }

        /// <summary>
        /// Cell index of the skill effect's second parameter.
        /// </summary>
        [JsonRequired]
        public int Parameter2 { get; set; }

        #endregion
    }
}
