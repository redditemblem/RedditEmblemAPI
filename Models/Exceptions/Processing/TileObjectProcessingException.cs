using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TileObjectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>TileObject</c>.
        /// </summary>
        public TileObjectProcessingException(string tileObjectName, Exception innerException)
            : base("tile object", tileObjectName, innerException)
        { }
    }
}
