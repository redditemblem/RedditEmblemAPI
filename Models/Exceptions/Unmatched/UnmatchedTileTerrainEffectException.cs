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
            : base($"The terrain effect \"{tileEffectName}\" located at coordinate \"{coord.X},{coord.Y}\" could not be matched to a known terrain effect definition. The given name must match exactly, including capitalization and punctuation.")
        { }
    }
}
