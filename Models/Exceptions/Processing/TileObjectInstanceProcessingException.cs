using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TileObjectInstanceProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>TileObjectInstance</c>.
        /// </summary>
        public TileObjectInstanceProcessingException(string tileObjectInstanceName, Exception innerException)
            : base("map object", tileObjectInstanceName, innerException)
        { }
    }
}
