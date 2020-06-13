using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace RedditEmblemAPI.Models.Output
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
        /// <exception cref="NegativeIntegerException"></exception>
        public ItemRange(int minimum, int maximum)
        {
            if (minimum < 0)
                throw new NegativeIntegerException("Minimum Range", minimum.ToString());
            if (maximum < 0)
                throw new NegativeIntegerException("Maximum Range", maximum.ToString());

            this.Minimum = minimum;
            this.Maximum = maximum;
        }


        /// <summary>
        /// Initializes the class with the passed in <paramref name="minimum"/> and <paramref name="maximum"/> values.
        /// </summary>
        /// <param name="minimum">A numerical string value.</param>
        /// <param name="maximum">A numerical string value.</param>
        /// <exception cref="NegativeIntegerException"></exception>
        public ItemRange(string minimum, string maximum)
        {
            int val;
            if (!int.TryParse(minimum, out val) || val < 0)
                throw new NegativeIntegerException("Minimum Range", minimum);
            this.Minimum = val;

            if (!int.TryParse(maximum, out val) || val < 0)
                throw new NegativeIntegerException("Maximum Range", maximum);
            this.Maximum = val;
        }
    }
}