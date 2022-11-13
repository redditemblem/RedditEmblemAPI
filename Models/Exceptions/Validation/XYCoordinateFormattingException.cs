using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class XYCoordinateFormattingException : Exception
    {
        /// <summary>
        /// Thrown when data is not found to be in an "x,y" format and cannot be converted to a <c>Coordinate</c> object.
        /// </summary>
        public XYCoordinateFormattingException(string coord)
            : base($"The coordinate \"{coord}\" could not be parsed. All coordinates should be either \"x,y\" - where x and y are non-zero, positive integers - or the exact name of another unit.")
        { }
    }
}
