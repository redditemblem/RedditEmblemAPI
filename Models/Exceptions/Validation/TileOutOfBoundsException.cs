using RedditEmblemAPI.Models.Output;
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
            : base(string.Format("Attempted to access map tile \"{0},{1}\" but it was not found.", coord.X, coord.Y))
        { }

        /// <summary>
        /// Thrown when attempting to access a map tile that not exist. 
        /// </summary>
        public TileOutOfBoundsException(int x, int y)
            : base(string.Format("Attempted to access map tile \"{0},{1}\" but it was not found.", x, y))
        { }
    }
}
