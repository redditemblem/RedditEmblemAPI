using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class MinimumGreaterThanMaximumException : Exception
    {
        /// <summary>
        /// Thrown when the minimum value in a value pair is greater than the maximum value.
        /// </summary>
        /// <param name="minimumValueDescription"></param>
        /// <param name="maximumValueDescription"></param>
        public MinimumGreaterThanMaximumException(string minimumValueDescription, string maximumValueDescription)
            : base(string.Format("The value of \"{0}\" cannot be less than the value of \"{1}\".", minimumValueDescription, maximumValueDescription))
        { }
    }
}
