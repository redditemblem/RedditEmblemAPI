using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public abstract class IntegerException : Exception
    {
        private const string Message = "Cell \"{0}\" contained the value \"{1}\" where {2} was expected.";

        public IntegerException(string cell, string actualValue, string expectedValue)
            : base(string.Format(Message, cell, actualValue, expectedValue))
        { }
    }
}
