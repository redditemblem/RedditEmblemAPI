using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapHeightException : Exception
    {
        /// <summary>
        /// Thrown when the number of vertical tiles fetched does not match the calculated number of vertical tiles from the map image.
        /// </summary>
        public UnexpectedMapHeightException(int actualHeight, int expectedHeight, string sheetName)
            : base($"{actualHeight} rows were found on the \"{sheetName}\" sheet when {expectedHeight} were expected. The number of rows should equal the tallest map segment's height in tiles.")
        { }
    }
}
