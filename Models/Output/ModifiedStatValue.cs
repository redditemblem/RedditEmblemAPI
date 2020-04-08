using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a stat value that can be modified.
    /// </summary>
    public class ModifiedStatValue
    {
        public ModifiedStatValue()
        {
            this.Modifiers = new Dictionary<string, int>();
        }

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
        public Dictionary<string, int> Modifiers { get; set; }
    }
}