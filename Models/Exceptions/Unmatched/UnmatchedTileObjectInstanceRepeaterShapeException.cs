using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileObjectInstanceRepeaterShapeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>TileObjectInstanceRepeaterShape</c>.
        /// </summary>
        public UnmatchedTileObjectInstanceRepeaterShapeException(string repeaterShape, IEnumerable<string> possibleValues)
            : base("repeater shape", repeaterShape, possibleValues)
        { }
    }
}
