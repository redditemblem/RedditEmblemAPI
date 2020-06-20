using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;

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
        /// Initializes the class with the passed in <paramref name="current"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="maximum"></param>
        /// <exception cref="PositiveIntegerException"></exception>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public HP(int current, int maximum)
        {
            if (current < 0)
                throw new PositiveIntegerException("Current HP", current.ToString());
            if (maximum <= 0)
                throw new NonZeroPositiveIntegerException("Maximum HP", maximum.ToString());

            this.Current = current;
            this.Maximum = maximum;
        }


        /// <summary>
        /// Initializes the class with the passed in <paramref name="current"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="current">A numerical value.</param>
        /// <param name="maximum">A numerical value.</param>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public HP(string current, string maximum)
        {
            int currentVal = ParseHelper.SafeIntParse(current, "Current HP", true);
            this.Current = currentVal;

            int maximumVal = ParseHelper.SafeIntParse(maximum, "Maximum HP", true);
            if(maximumVal == 0)
                throw new NonZeroPositiveIntegerException("Maximum HP", maximum);
            this.Maximum = maximumVal;
        }

        #endregion
    }
}
