using RedditEmblemAPI.Models.Output.Map;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class TileOutOfBoundsException : Exception
    {
        /// <summary>
        /// Thrown when attempting to access a map tile that not exist. 
        /// </summary>
        /// <param name="coord"></param>
        public TileOutOfBoundsException(ICoordinate coord)
            : base($"Attempted to access map tile \"{coord.AsText}\" but it could not be found. Is there a multi-tile entity overlapping the map boundaries?")
        { }
    }
}
