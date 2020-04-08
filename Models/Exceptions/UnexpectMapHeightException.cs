using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnexpectedMapHeightException : Exception
    {
        public UnexpectedMapHeightException(int actualHeight, int expectedHeight)
            : base(string.Format("{0} mapped tile rows were found when {1} was expected.", actualHeight, expectedHeight))
        { }
    }
}
