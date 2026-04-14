using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Skills
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Effect"</c> object data.
    /// </summary>
    public class SkillEffectConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a skill effect's type.
        /// </summary>
        [JsonRequired]
        public (int, int) Type { get; set; }

        /// <summary>
        /// Required. Collection of locations of a skill effect's parameters.
        /// </summary>
        [JsonRequired]
        public (int, int)[] Parameters { get; set; }

        #endregion Required Fields
    }
}
