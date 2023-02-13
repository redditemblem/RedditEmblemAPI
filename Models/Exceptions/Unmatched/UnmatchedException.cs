using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    /// <summary>
    /// Abstract. An error thrown when a text value cannot be matched with an object.
    /// </summary>
    public abstract class UnmatchedException : Exception
    {
        public UnmatchedException(string description, string unmatchedValue)
            : base($"The value \"{unmatchedValue}\" could not be matched to a known {description} definition. The given name must match exactly, including capitalization and punctuation.")
        { }

        public UnmatchedException(string description, string unmatchedValue, IEnumerable<string> possibleValues)
            : base($"The value \"{unmatchedValue}\" could not be matched to a known {description}. Possible values include: {string.Join(", ", possibleValues)}.")
        { }
    }
}
