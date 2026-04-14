using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

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
        /// Required. Collection of equation configs.
        /// </summary>
        [JsonRequired]
        public CalculatedStatEquationConfig[] Equations { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. The method by which the correct equation is selected.
        /// </summary>
        public CalculatedStatEquationSelectorEnum SelectsUsing { get; set; } = CalculatedStatEquationSelectorEnum.None;

        /// <summary>
        /// Optional. List of named modifiers to this stat. (ex. "Buff/Debuff")
        /// </summary>
        public NamedStatConfig[] Modifiers { get; set; } = Array.Empty<NamedStatConfig>();

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
