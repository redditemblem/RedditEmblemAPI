using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class MapProcessingException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing the map.
        /// </summary>
        /// <param name="innerException"></param>
        public MapProcessingException(Exception innerException)
            : base("An error occurred while processing the map.", innerException)
        { }
    }
}
