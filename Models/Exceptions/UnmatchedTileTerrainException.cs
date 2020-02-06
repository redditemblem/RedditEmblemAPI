using RedditEmblemAPI.Models.Common;
using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedTileTerrainException : Exception
    {
        public UnmatchedTileTerrainException(Coordinate coordinate, string terrainName)
            : base(string.Format("The terrain type \"{0}\" located at coordinate \"({1},{2})\" could not be matched to a known terrain type definition. The given name must match exactly, including capitalization and punctuation.",
                   terrainName, coordinate.X, coordinate.Y
                  ))
        { }
    }
}
