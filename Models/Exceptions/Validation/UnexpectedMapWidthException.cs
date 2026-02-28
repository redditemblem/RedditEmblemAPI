using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapWidthException : Exception
    {
        /// <summary>
        /// Thrown when the number of horizontal tiles fetched does not match the calculated number of horizontal tiles from a map segment's image.
        /// </summary>
        public UnexpectedMapWidthException(string segmentTitle, int rowNumber, int actualWidth, int expectedWidth, string sheetName)
            : base($"{actualWidth} tiles were found for segment \"{segmentTitle}\" in row {rowNumber} of the \"{sheetName}\" sheet. {expectedWidth} tiles were expected.")
        { }
    }
}
