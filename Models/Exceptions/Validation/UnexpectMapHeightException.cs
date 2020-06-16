using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapHeightException : Exception
    {
        /// <summary>
        /// Thrown when the number of vertical tiles fetched from the "Map Tiles" sheet does not match the calculated number of vertical tiles from the map image.
        /// </summary>
        /// <param name="actualWidth"></param>
        /// <param name="expectedWidth"></param>
        public UnexpectedMapHeightException(int actualHeight, int expectedHeight)
            : base(string.Format("{0} mapped tile rows were found when {1} were expected.", actualHeight, expectedHeight))
        { }
    }
}
