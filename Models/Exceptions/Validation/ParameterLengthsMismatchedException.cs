using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class ParameterLengthsMismatchedException : Exception
    {
        /// <summary>
        /// Thrown when a set of stat and value parameter lists have differing lengths.
        /// </summary>
        public ParameterLengthsMismatchedException(params string[] parameterFieldNames)
            : base($"Parameters \"{string.Join(", ", parameterFieldNames)}\" must contain the same number of items.")
        { }
    }
}
