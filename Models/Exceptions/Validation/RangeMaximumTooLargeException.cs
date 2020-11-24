using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class RangeMaximumTooLargeException : Exception
    {
        public RangeMaximumTooLargeException(string message)
            : base(message)
        { }
    }
}
