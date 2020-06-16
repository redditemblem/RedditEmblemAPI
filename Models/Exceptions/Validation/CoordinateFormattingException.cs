using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class CoordinateFormattingException : Exception
    {
        /// <summary>
        /// Thrown when data is not found to be in an "x,y" format and cannot be converted to a <c>Coordinate</c> object.
        /// </summary>
        public CoordinateFormattingException(string coord)
            : base(string.Format("The coordinate \"{0}\" could not be parsed. All coordinates should be in \"x,y\" format, where x and y are non-zero, positive numbers.", coord)) 
        { }
    }
}
