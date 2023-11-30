using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Struct representing the value of a named stat, plus a little extra data for controlling its display.
    /// </summary>
    public class NamedStatValue
    {
        #region Attributes

        /// <summary>
        /// The value of the stat.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// For the UI. Flag indicating if the normal positive/negative modified colors should be inverted.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InvertModifiedDisplayColors { get; private set; }

        #endregion Attributes

        public NamedStatValue(decimal value)
        {
            this.Value = value;
            this.InvertModifiedDisplayColors = false;
        }

        public NamedStatValue(decimal value, bool invertModifiedDisplayColors)
        {
            this.Value = value;
            this.InvertModifiedDisplayColors = invertModifiedDisplayColors;
        }
    }
}
