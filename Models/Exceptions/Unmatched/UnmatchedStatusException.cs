using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedStatusException : Exception
    {
        public UnmatchedStatusException(string statusName)
            : base(string.Format("The status condition \"{0}\" could not be matched to a known status definition. The given name must match exactly, including capitalization and punctuation.", statusName))
        { }
    }
}
