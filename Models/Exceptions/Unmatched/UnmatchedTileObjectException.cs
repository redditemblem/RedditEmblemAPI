using RedditEmblemAPI.Models.Output.Map;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileObjectException : Exception
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>TileObject</c>.
        /// </summary>
        public UnmatchedTileObjectException(ICoordinate coord, string tileObjectName)
            : base($"The tile object \"{tileObjectName}\" located at coordinate \"{coord.AsText}\" could not be matched to a known tile object definition. The given name must match exactly, including capitalization and punctuation.")
        { }
    }
}
