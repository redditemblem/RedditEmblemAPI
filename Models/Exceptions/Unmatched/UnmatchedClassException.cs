using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedClassException : Exception
    {
        public UnmatchedClassException(string className)
            : base(string.Format("The class \"{0}\" could not be matched to a known class definition. The given name must match exactly, including capitalization and punctuation.", className))
        { }
    }
}
