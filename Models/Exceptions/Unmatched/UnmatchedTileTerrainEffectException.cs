using RedditEmblemAPI.Models.Output.Map;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileTerrainEffectException : Exception
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>TerrainEffect</c>.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="tileEffectName"></param>
        public UnmatchedTileTerrainEffectException(Coordinate coord, string tileEffectName)
            : base(string.Format("The terrain effect \"{0}\" located at coordinate \"{1},{2}\" could not be matched to a known terrain effect definition. The given name must match exactly, including capitalization and punctuation.",
                   tileEffectName, coord.X, coord.Y
                  ))
        { }
    }
}
