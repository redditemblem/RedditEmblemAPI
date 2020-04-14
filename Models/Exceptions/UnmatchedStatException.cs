using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedStatException : Exception
    {
        public UnmatchedStatException(string statName)
            : base(string.Format("Could not locate a stat with the name \"{0}\".", statName))
        { }
    }
}
