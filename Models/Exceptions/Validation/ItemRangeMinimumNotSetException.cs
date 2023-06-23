using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class ItemRangeMinimumNotSetException : Exception
    {
        /// <summary>
        /// Thrown when an item has a minimum range of 0 and a maximum range > 0.
        /// </summary>
        public ItemRangeMinimumNotSetException(string minRangeLabel, string maxRangeLabel)
            : base($"{minRangeLabel} cannot be 0 if the {maxRangeLabel} is greater than 0.")
        { }
    }
}