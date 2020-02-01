using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public abstract class IntegerException : Exception
    {
        public IntegerException(string Value, string actualValue, string expectedValue)
            : base(string.Format("Value \"{0}\" contained the value \"{1}\" where {2} was expected.", Value, actualValue, expectedValue))
        { }
    }
}
