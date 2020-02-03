using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedItemException : Exception
    {
        public UnmatchedItemException(string itemName)
            : base(string.Format("The item \"{0}\" could not be matched to an item definition. The given name must match exactly, including capitalization and punctuation.", itemName))
        { }
    }
}
