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
        #region RequiredFields

        [JsonRequired]
        public WorksheetQuery WorksheetQuery { get; set; }

        /// <summary>
        /// Cell index of a skill's name value.
        /// </summary>
        [JsonRequired]
        public int SkillName { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// Cell index of a skill's sprite image URL value.
        /// </summary>
        public int SpriteURL { get; set; } = -1;

        /// <summary>
        /// List of cell indexes for a skill's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
