using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class MapHasNoSegmentsException : Exception
    {
        public MapHasNoSegmentsException()
            : base($"No map segments could be created for the map. Have map image URLs been set up on your system sheet?")
        { }
    }
}
