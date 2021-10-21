using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class HexCodeException : Exception
    {
        /// <summary>
        /// Thrown when a string cannot be parsed to a hex color code.
        /// </summary>
        public HexCodeException(string fieldName, string value)
            : base($"The field \"{fieldName}\" contained the value \"{value}\" where a hex color code was expected. Color codes should start with \"#\" and contain 6 characters from the sets of 0-9, A-F.")
        { }
    }
}
