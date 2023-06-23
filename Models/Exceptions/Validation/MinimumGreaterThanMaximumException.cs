using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class MinimumGreaterThanMaximumException : Exception
    {
        /// <summary>
        /// Thrown when the minimum value in a value pair is greater than the maximum value.
        /// </summary>
        public MinimumGreaterThanMaximumException(string minimumValueDescription, string maximumValueDescription)
            : base($"The value of \"{minimumValueDescription}\" cannot be greater than the value of \"{maximumValueDescription}\".")
        { }
    }
}
