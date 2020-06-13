using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnexpectedMapHeightException : Exception
    {
        public UnexpectedMapHeightException(int actualHeight, int expectedHeight)
            : base(string.Format("{0} mapped tile rows were found when {1} were expected.", actualHeight, expectedHeight))
        { }
    }
}
