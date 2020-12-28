using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedWarpTypeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>WarpType</c> enum.
        /// </summary>
        /// <param name="warpType"></param>
        public UnmatchedWarpTypeException(string warpType)
            : base("warp type", warpType, new List<string>() { "Entrance", "Exit", "Dual" })
        { }
    }
}
