using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"System"</c> object data.
    /// </summary>
    public class SystemConfig
    {
        #region RequiredValues

        /// <summary>
        /// Container object for a system's item configuration.
        /// </summary>
        [JsonRequired]
        public ItemsConfig Items { get; set; }

        /// <summary>
        /// Container object for a system's skill configuration.
        /// </summary>
        [JsonRequired]
        public SkillsConfig Skills { get; set; }

        /// <summary>
        /// Container object for a system's class configuration.
        /// </summary>
        [JsonRequired]
        public ClassesConfig Classes { get; set; }

        /// <summary>
        /// Container object for a system's terrain type configuration.
        /// </summary>
        [JsonRequired]
        public TerrainTypesConfig TerrainTypes { get; set; }

        #endregion
    }
}
