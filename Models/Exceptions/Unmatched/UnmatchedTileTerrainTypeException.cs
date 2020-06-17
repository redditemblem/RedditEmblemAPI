using RedditEmblemAPI.Models.Output.Map;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileTerrainTypeException : Exception
    {
        private const string ERROR = "The terrain type \"{0}\" located at coordinate \"{1},{2}\" could not be matched to a known terrain type definition. The given name must match exactly, including capitalization and punctuation.";
        
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>TerrainType</c>.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="terrainName"></param>
        public UnmatchedTileTerrainTypeException(Coordinate coord, string terrainName)
            : base(string.Format(ERROR, terrainName, coord.X, coord.Y ))
        { }

        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>TerrainType</c>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="terrainName"></param>
        public UnmatchedTileTerrainTypeException(int x, int y, string terrainName)
            : base(string.Format(ERROR, terrainName, x, y))
        { }
    }
}
