using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class InvalidWarpGroupException : Exception
    {
        /// <summary>
        /// Thrown when a warp group does not have a valid number of entrances/exits.
        /// </summary>
        /// <param name="warpGroupNum"></param>
        public InvalidWarpGroupException(string warpGroupNum)
            : base($"Warp group \"{warpGroupNum}\" must contain at least one entrance and one exit to be valid.")
        { }
    }
}
