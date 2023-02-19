using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Object representing a stat value that can be modified.
    /// </summary>
    public class ModifiedStatValue
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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModifiedStatValue()
        {
            this.Modifiers = new Dictionary<string, int>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="baseValue"></param>
        public ModifiedStatValue(int baseValue)
        {
            this.BaseValue = baseValue;
            this.Modifiers = new Dictionary<string, int>();
        }

        #endregion
    }
}