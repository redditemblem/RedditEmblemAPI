using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class ItemRangeMaximumTooLargeException : Exception
    {
        /// <summary>
        /// Thrown when an item range exceeds <paramref name="maxTileCount"/> tiles.
        /// </summary>
        /// <param name="maxTileCount"></param>
        public ItemRangeMaximumTooLargeException(int maxTileCount)
            : base($"For performance reasons, item ranges in excess of {maxTileCount} tiles are currently not allowed.")
        { }
    }
}
