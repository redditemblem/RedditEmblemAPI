using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a <c>Unit</c>'s HP stats. 
    /// </summary>
    public class HP
    {
        /// <summary>
        /// The current number of hit points a unit has.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// The maximum number of hit points a unit has.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The percentage of hit points the unit has remaining.
        /// </summary>
        public decimal Percentage { get { return Math.Round((decimal)this.Current / this.Maximum, 2) * 100; } }

        #region Constructors

        /// <summary>
        /// Initializes the class with the values in <paramref name="data"/> at <paramref name="currentIndex"/> and <paramref name="maximumIndex"/>.
        /// </summary>
        public HP(List<string> data, int currentIndex, int maximumIndex)
        {
            int currentVal = DataParser.Int_Positive(data, currentIndex, "Current HP");
            this.Current = currentVal;

            int maximumVal = DataParser.Int_NonZeroPositive(data, maximumIndex, "Maximum HP");
            this.Maximum = maximumVal;
        }

        #endregion
    }
}
