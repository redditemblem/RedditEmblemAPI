using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    #region Interface

    /// <inheritdoc cref="ModifiedStatValue"/>
    public interface IModifiedStatValue
    {
        /// <inheritdoc cref="ModifiedStatValue.BaseValue"/>
        int BaseValue { get; set; }

        /// <inheritdoc cref="ModifiedStatValue.FinalValue"/>
        int FinalValue { get; }

        /// <inheritdoc cref="ModifiedStatValue.Modifiers"/>
        IDictionary<string, int> Modifiers { get; set; }

        /// <inheritdoc cref="ModifiedStatValue.InvertModifiedDisplayColors"/>
        bool InvertModifiedDisplayColors { get; }

        /// <inheritdoc cref="ModifiedStatValue.UsePrioritizedDisplay"/>
        bool UsePrioritizedDisplay { get; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a stat value that can be modified.
    /// </summary>
    public class ModifiedStatValue : IModifiedStatValue
    {
        #region Attributes

        /// <summary>
        /// The base value of the stat without any modifiers applied.
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// Returns the stat's <c>BaseValue</c> summed with all values in the <c>Modifiers</c> list.
        /// </summary>
        public int FinalValue { get { return this.BaseValue + this.Modifiers.Sum(m => m.Value); } }

        /// <summary>
        /// Collection of all values that modify this stat. (ex. "Debuff",-2)
        /// </summary>
        public IDictionary<string, int> Modifiers { get; set; }

        /// <summary>
        /// For the UI. Flag indicating if the normal positive/negative modified colors should be inverted.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool InvertModifiedDisplayColors { get; private set; }

        /// <summary>
        /// For the UI. Flag indicating that this stat should be shown as prioritized in the UI.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool UsePrioritizedDisplay { get; private set; }

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModifiedStatValue(bool invertModifiedDisplayColors, bool usePrioritizedDisplay)
        {
            this.Modifiers = new Dictionary<string, int>();
            this.InvertModifiedDisplayColors = invertModifiedDisplayColors;
            this.UsePrioritizedDisplay = usePrioritizedDisplay;
        }

        #endregion Constructors
    }
}