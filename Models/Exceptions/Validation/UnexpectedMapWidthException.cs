using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapWidthException : Exception
    {
        /// <summary>
        /// Thrown when the number of horizontal tiles fetched from the "Map Tiles" sheet does not match the calculated number of horizontal tiles from the map image.
        /// </summary>
        /// <param name="actualWidth"></param>
        /// <param name="expectedWidth"></param>
        public UnexpectedMapWidthException(int actualWidth, int expectedWidth)
            : base(string.Format("{0} mapped tiles were found in a row when {1} were expected.", actualWidth, expectedWidth))
        { }
    }
}
