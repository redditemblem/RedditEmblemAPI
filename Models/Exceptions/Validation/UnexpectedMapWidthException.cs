using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapWidthException : Exception
    {
        /// <summary>
        /// Thrown when the number of horizontal tiles fetched from a tiles sheet does not match the calculated number of horizontal tiles from the map image.
        /// </summary>
        /// <param name="actualWidth"></param>
        /// <param name="expectedWidth"></param>
        /// <param name="sheetName"></param>
        public UnexpectedMapWidthException(int actualWidth, int expectedWidth, string sheetName)
            : base(string.Format("{0} tiles were found in a row from the \"{1}\" sheet when {2} were expected.", actualWidth, sheetName, expectedWidth))
        { }
    }
}
