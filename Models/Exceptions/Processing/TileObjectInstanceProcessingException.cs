using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TileObjectInstanceProcessingException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>TileObjectInstance</c>.
        /// </summary>
        public TileObjectInstanceProcessingException(string tileObjectInstanceName, string coordinate, Exception innerException)
            : base($"An error occurred while processing map object \"{tileObjectInstanceName}\" located at coordinate \"{coordinate}\".", innerException)
        { }
    }
}
