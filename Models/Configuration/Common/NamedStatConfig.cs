using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON object data representing a named stat with a fixed value.
    /// </summary>
    public class NamedStatConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Name of the stat value. (ex. "Str")
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Required. Cell index for the stat's value.
        /// </summary>
        [JsonRequired]
        public int Value { get; set; }

        #endregion Required Fields
    }

    /// <summary>
    /// Inherits <c>NamedStatConfig</c>. Contains extra config options for controlling how this stat is displayed in the UI.
    /// </summary>
    public class NamedStatConfig_Displayed : NamedStatConfig
    {
        #region Optional Fields

        /// <summary>
        /// Optional, defaults to false. Flag indicating if the normal positive/negative modified colors in the UI for this stat should be inverted.
        /// </summary>
        public bool InvertModifiedDisplayColors { get; set; } = false;

        #endregion Optional Fields
    }
}