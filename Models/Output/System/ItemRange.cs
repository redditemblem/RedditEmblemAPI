using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing an <c>Item</c>'s range.
    /// </summary>
    public class ItemRange
    {
        /// <summary>
        /// The minimum number of tiles an item can reach.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// The maximum number of tiles an item can reach.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Initializes the class with the passed in <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <exception cref="PositiveIntegerException"></exception>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public ItemRange(int minimum, int maximum)
        {
            if (minimum < 0)
                throw new PositiveIntegerException("Minimum Range", minimum.ToString());
            if (maximum < 0)
                throw new PositiveIntegerException("Maximum Range", maximum.ToString());
            if (minimum > maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
            if (maximum > 15)
                throw new RangeMaximumTooLargeException("For performance reasons, item ranges in excess of 15 tiles are currently not allowed.");

            this.Minimum = minimum;
            this.Maximum = maximum;
        }


        /// <summary>
        /// Initializes the class with the passed in <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum">A numerical string value.</param>
        /// <param name="maximum">A numerical string value.</param>
        /// <exception cref="MinimumGreaterThanMaximumException"></exception>
        public ItemRange(string minimum, string maximum)
        {
            this.Minimum = ParseHelper.SafeIntParse(minimum, "Minimum Range", true);
            this.Maximum = ParseHelper.SafeIntParse(maximum, "Maximum Range", true);

            if (this.Minimum > this.Maximum)
                throw new MinimumGreaterThanMaximumException("Minimum Range", "Maximum Range");
            if (this.Maximum > 15)
                throw new RangeMaximumTooLargeException("For performance reasons, item ranges in excess of 15 tiles are currently not allowed.");
        }
    }
}