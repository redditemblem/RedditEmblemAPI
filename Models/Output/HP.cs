using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Exceptions.Validation;
using System;

namespace RedditEmblemAPI.Models.Output
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
        /// The percentage of HP the unit has remaining.
        /// </summary>
        public decimal Percentage { get { return Math.Round((decimal)this.Current / this.Maximum, 2) * 100; } }

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
        /// <param name="current">A numerical string value.</param>
        /// <param name="maximum">A numerical string value.</param>
        /// <exception cref="PositiveIntegerException"></exception>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public HP(string current, string maximum)
        {
            int val;
            if (!int.TryParse(current, out val) || val < 0)
                throw new PositiveIntegerException("Current HP", current);
            this.Current = val;

            if (!int.TryParse(maximum, out val) || val <= 0)
                throw new NonZeroPositiveIntegerException("Maximum HP", maximum);
            this.Maximum = val;
        }
    }
}
