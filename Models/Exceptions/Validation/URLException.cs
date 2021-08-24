using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class URLException : Exception
    {
        /// <summary>
        /// Thrown when a string cannot be parsed to a web URL.
        /// </summary>
        public URLException(string fieldName, string value)
            : base($"The field \"{fieldName}\" contained the value \"{value}\" where a web URL was expected.")
        { }
    }
}
