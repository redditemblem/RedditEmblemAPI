using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    internal class MapImageLoadFailedException : Exception
    {
        /// <summary>
        /// Constructor for when the image load failure is the result of a 404 not found error.
        /// </summary>
        public MapImageLoadFailedException() 
            : base($"The map image load failed with a \"404 Not Found\" message. Does the map image URL point to a valid and accessible image file?")
        { }

        /// <summary>
        /// Constructor for when the image load failure is NOT the result of a 404 not found error.
        /// </summary>
        public MapImageLoadFailedException(string exceptionMessage) 
            : base($"The map image load failed with the following message: {exceptionMessage}")
        { }
    }
}
