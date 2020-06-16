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
            : base(string.Format("The value \"{0}\" could not be matched to a known {1} definition. The given name must match exactly, including capitalization and punctuation.", unmatchedValue, description))
        { }

        public UnmatchedException(string description, string unmatchedValue, IList<string> possibleValues)
            : base(string.Format("The value \"{0}\" could not be matched to a known {1}. Possible values include: {2}.", unmatchedValue, description, string.Join(", ", possibleValues)))
        { }
    }
}
