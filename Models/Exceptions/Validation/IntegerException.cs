using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    /// <summary>
    /// Abstract. An error thrown when a numerical value fails to parse.
    /// </summary>
    public abstract class IntegerException : Exception
    {
        public IntegerException(string fieldName, string actualValue, string expectedValue)
            : base($"The field \"{fieldName}\" contained the value \"{actualValue}\" where {expectedValue} was expected.")
        { }
    }
}
