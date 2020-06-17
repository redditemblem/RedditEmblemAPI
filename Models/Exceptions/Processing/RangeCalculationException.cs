using RedditEmblemAPI.Models.Output.Units;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class RangeCalculationException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs during the calculation of a unit's range(s).
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="innerException"></param>
        public RangeCalculationException(Unit unit, Exception innerException) 
            : base(string.Format("An error occurred while calculating map ranges for unit \"{0}\".", unit.Name), innerException)
        { }
    }
}
