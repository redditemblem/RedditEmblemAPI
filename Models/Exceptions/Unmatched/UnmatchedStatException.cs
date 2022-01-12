using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatException : Exception
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a configured stat.
        /// </summary>
        /// <param name="statName"></param>
        public UnmatchedStatException(string statName)
            : base($"Could not locate a stat with the name \"{statName}\".")
        { }
    }
}
