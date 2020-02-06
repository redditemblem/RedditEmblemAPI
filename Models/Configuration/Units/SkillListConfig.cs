using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Skills"</c> object data.
    /// </summary>
    public class SkillListConfig
    {
        #region RequiredFields

        /// <summary>
        /// List of cell indexes for a unit's skill names.
        /// </summary>
        [JsonRequired]
        public IList<int> Slots;

        #endregion
    }
}