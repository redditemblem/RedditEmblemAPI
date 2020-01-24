using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public abstract class IntegerException : Exception
    {
        public IntegerException(string cell, string actualValue, string expectedValue)
            : base(string.Format("Cell \"{0}\" contained the value \"{1}\" where {2} was expected.", cell, actualValue, expectedValue))
        { }
    }
}
