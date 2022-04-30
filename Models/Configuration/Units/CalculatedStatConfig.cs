using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units.CalculatedStats
{
    public class CalculatedStatConfig
    {
        #region Required Fields

        /// <summary>
        /// Name of the calculated stat. (Ex. Atk)
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Dynamically-executed equation to calculate the stat's value.
        /// </summary>
        [JsonRequired]
        public string Equation { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. List of named modifiers to this stat. (ex. "Buff/Debuff")
        /// </summary>
        public List<NamedStatConfig> Modifiers { get; set; } = new List<NamedStatConfig>();

        #endregion
    }
}
