using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedAffiliationException : Exception
    {
        public UnmatchedAffiliationException(string affiliation)
            : base(string.Format("The affiliation \"{0}\" could not be matched to a known class definition. The given name must match exactly, including capitalization and punctuation.", affiliation))
        { }
    }
}
