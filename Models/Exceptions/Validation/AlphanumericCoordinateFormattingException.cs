using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class AlphanumericCoordinateFormattingException : Exception
    {
        /// <summary>
        /// Thrown when data is not found to be in an "A1" format and cannot be converted to a <c>Coordinate</c> object.
        /// </summary>
        public AlphanumericCoordinateFormattingException(string coord)
            : base($"The coordinate \"{coord}\" could not be parsed. All coordinates should be either alphanumeric (ex. \"A1\") or the exact name of another unit.")
        { }
    }
}
