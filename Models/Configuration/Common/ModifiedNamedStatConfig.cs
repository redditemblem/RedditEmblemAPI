using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON object data representing a named stat with possible modifiers.
    /// </summary>
    public class ModifiedNamedStatConfig
    {
        #region Required Fields

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

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. List of named modifiers to this stat. (ex. "Buff/Debuff")
        /// </summary>
        public List<NamedStatConfig> Modifiers { get; set; } = new List<NamedStatConfig>();

        #endregion Optional Fields
    }

    /// <summary>
    /// Inherits <c>ModifiedNamedStatConfig</c>. Contains extra config options for controlling how this stat is displayed in the UI.
    /// </summary>
    public class ModifiedNamedStatConfig_Displayed : ModifiedNamedStatConfig
    {
        #region Optional Attributes

        /// <summary>
        /// Optional, defaults to false. Flag indicating if the normal positive/negative modified colors in the UI for this stat should be inverted.
        /// </summary>
        public bool InvertModifiedDisplayColors { get; set; } = false;

        #endregion Optional Attributes
    }
}