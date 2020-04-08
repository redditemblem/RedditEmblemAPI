using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnexpectedMapWidthException : Exception
    {
        public UnexpectedMapWidthException(int actualWidth, int expectedWidth)
            : base(string.Format("{0} mapped tiles were found in a row when {1} was expected.", actualWidth, expectedWidth))
        { }
    }
}
