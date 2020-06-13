using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedEquippedItemException : Exception
    {
        public UnmatchedEquippedItemException(string itemName)
            : base(string.Format("The equipped item \"{0}\" does not match any of the items in the unit's inventory. The given name must match exactly, including capitalization and punctuation.", itemName))
        { }
    }
}
