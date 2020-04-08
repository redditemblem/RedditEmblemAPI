using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public abstract class IntegerException : Exception
    {
        public IntegerException(string fieldName, string actualValue, string expectedValue)
            : base(string.Format("The field \"{0}\" contained the value \"{1}\" where {2} was expected.", fieldName, actualValue, expectedValue))
        { }
    }
}
