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
        public TileOutOfBoundsException(Coordinate coord)
            : base($"Attempted to access map tile \"{coord.X},{coord.Y}\" but it was not found. Is there a multi-tile entity overlapping the map boundaries?")
        { }

        /// <summary>
        /// Thrown when attempting to access a map tile that not exist. 
        /// </summary>
        public TileOutOfBoundsException(int x, int y)
            : base($"Attempted to access map tile \"{x},{y}\" but it was not found. Is there a multi-tile entity overlapping the map boundaries?")
        { }
    }
}
