using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapHeightException : Exception
    {
        /// <summary>
        /// Thrown when the number of vertical tiles fetched from a tiles sheet does not match the calculated number of vertical tiles from the map image.
        /// </summary>
        /// <param name="actualHeight"></param>
        /// <param name="expectedHeight"></param>
        /// <param name="sheetName"></param>
        public UnexpectedMapHeightException(int actualHeight, int expectedHeight, string sheetName)
            : base($"{actualHeight} tile rows were found on the \"{sheetName}\" sheet when {expectedHeight} were expected.")
        { }
    }
}
