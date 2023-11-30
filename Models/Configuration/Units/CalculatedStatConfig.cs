using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units.CalculatedStats
{
    public class CalculatedStatConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Name of the calculated stat. (Ex. Atk)
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Required. List of equation configs.
        /// </summary>
        [JsonRequired]
        public List<CalculatedStatEquationConfig> Equations { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. The method by which the correct equation is selected.
        /// </summary>
        public CalculatedStatEquationSelectorEnum SelectsUsing { get; set; } = CalculatedStatEquationSelectorEnum.None;

        /// <summary>
        /// Optional. List of named modifiers to this stat. (ex. "Buff/Debuff")
        /// </summary>
        public List<NamedStatConfig> Modifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional, defaults to false. Flag indicating if the normal positive/negative modified colors in the UI for this stat should be inverted.
        /// </summary>
        public bool InvertModifiedDisplayColors { get; set; } = false;

        #endregion Optional Fields
    }

    public enum CalculatedStatEquationSelectorEnum
    {
        None = 0,
        EquippedWeaponUtilizedStat = 1
    }
}
