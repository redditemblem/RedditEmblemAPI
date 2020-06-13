using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedItemException : Exception
    {
        public UnmatchedItemException(string itemName)
            : base(string.Format("The item \"{0}\" could not be matched to a known item definition. The given name must match exactly, including capitalization and punctuation.", itemName))
        { }
    }
}
