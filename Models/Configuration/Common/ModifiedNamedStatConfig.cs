using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON object data.
    /// </summary>
    public class ModifiedNamedStatConfig
    {
        #region RequiredFields

        /// <summary>
        /// Required. Name of the stat value. (ex. "Str")
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Required. Cell index for the stat's base value without any modifiers applied.
        /// </summary>
        [JsonRequired]
        public int BaseValue { get; set; }

        /// <summary>
        /// Required. List of named modifiers to this stat. (ex. "Buff/Debuff")
        /// </summary>
        [JsonRequired]
        public IList<NamedStatConfig> Modifiers { get; set; }

        #endregion
    }
}
