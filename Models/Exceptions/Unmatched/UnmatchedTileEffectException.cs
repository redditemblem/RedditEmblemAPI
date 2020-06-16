using RedditEmblemAPI.Models.Output;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileEffectException : Exception
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>TerrainEffect</c>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="effectName"></param>
        public UnmatchedTileEffectException(Coordinate coord, string effectName)
            : base(string.Format("The terrain effect \"{0}\" located at coordinate \"{1},{2}\" could not be matched to a known terrain effect definition. The given name must match exactly, including capitalization and punctuation.",
                   effectName, coord.X, coord.Y
                  ))
        { }
    }
}
