using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class HexException : Exception
    {
        /// <summary>
        /// Thrown when a string cannot be parsed to a hex code.
        /// </summary>
        public HexException(string fieldName, string value)
            : base($"The field \"{fieldName}\" contained the value \"{value}\" where a hex color code was expected. Hex color codes are 6 characters long and consist of the digits 0-9 and letters A-F. Optionally, they may start with a # symbol.")
        { }
    }
}