using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Output
{
    #region Interface

    /// <inheritdoc cref="NamedStatValue"/>
    public interface INamedStatValue
    {
        /// <inheritdoc cref="NamedStatValue.Value"/>
        decimal Value { get; }

        /// <inheritdoc cref="NamedStatValue.InvertModifiedDisplayColors"/>
        bool InvertModifiedDisplayColors { get; }
    }

    #endregion Interface

    /// <summary>
    /// Struct representing the value of a named stat, plus a little extra data for controlling its display.
    /// </summary>
    public readonly struct NamedStatValue : INamedStatValue
    {
        #region Attributes

        /// <summary>
        /// The value of the stat.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// For the UI. Flag indicating if the normal positive/negative modified colors should be inverted.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InvertModifiedDisplayColors { get; }

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
