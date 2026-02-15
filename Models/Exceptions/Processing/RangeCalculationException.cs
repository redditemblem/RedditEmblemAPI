using RedditEmblemAPI.Models.Output.Units;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class RangeCalculationException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs during the calculation of a unit's range(s).
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="innerException"></param>
        public RangeCalculationException(IUnit unit, Exception innerException)
            : base($"An error occurred while calculating map ranges for unit \"{unit.Name}\".", innerException)
        { }
    }
}
