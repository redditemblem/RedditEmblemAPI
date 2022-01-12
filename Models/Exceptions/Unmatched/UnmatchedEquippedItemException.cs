using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    /// <summary>
    /// Thrown when a unit's equipped item cannot be matched to any of the items in the unit's inventory.
    /// </summary>
    public class UnmatchedEquippedItemException : Exception
    {
        public UnmatchedEquippedItemException(string itemName)
            : base($"The equipped item \"{itemName}\" does not match any of the items in the unit's inventory. The given name must match exactly, including capitalization and punctuation.")
        { }
    }
}
