using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedShopItemException : Exception
    {
        public UnmatchedShopItemException(string itemName)
            : base(string.Format("The shop item \"{0}\" could not be matched to a known item definition. The given name must match exactly, including capitalization and punctuation.", itemName))
        { }
    }
}
