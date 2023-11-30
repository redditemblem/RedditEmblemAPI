using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitInventoryItemStat
    {
        #region Attributes

        /// <summary>
        /// The base value of the stat without any modifiers applied.
        /// </summary>
        public decimal BaseValue { get; set; }

        /// <summary>
        /// Returns the stat's <c>BaseValue</c> summed with <c>ForcedModifier</c> OR all items in the <c>Modifiers</c> list.
        /// </summary>
        public decimal FinalValue { get { return this.BaseValue + (this.ForcedModifier != 0 ? this.ForcedModifier : this.Modifiers.Sum(m => m.Value)); } }

        /// <summary>
        /// Collection of all values that modify this stat. (ex. "Debuff",-2)
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, int> Modifiers { get; set; }

        /// <summary>
        /// For use with logic that forces a final value for the stat, like skill effects. If used, the normal <c>this.Modifiers</c> list will be ignored.
        /// </summary>
        [JsonIgnore]
        public int ForcedModifier { get; set; }

        /// <summary>
        /// For the UI. Flag indicating if the normal positive/negative modified colors should be inverted.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private bool InvertModifiedDisplayColors { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitInventoryItemStat()
        {
            this.Modifiers = new Dictionary<string, int>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitInventoryItemStat(decimal baseValue)
        {
            this.BaseValue = baseValue;
            this.InvertModifiedDisplayColors = false;
            this.Modifiers = new Dictionary<string, int>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitInventoryItemStat(decimal baseValue, bool invertModifiedDisplayColors)
        {
            this.BaseValue = baseValue;
            this.InvertModifiedDisplayColors = invertModifiedDisplayColors;
            this.Modifiers = new Dictionary<string, int>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitInventoryItemStat(NamedStatValue stat)
        {
            this.BaseValue = stat.Value;
            this.InvertModifiedDisplayColors = stat.InvertModifiedDisplayColors;
            this.Modifiers = new Dictionary<string, int>();
        }

    }
}
