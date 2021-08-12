using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitPairedWithSelfException : Exception
    {
        public UnitPairedWithSelfException(string unitName)
            : base($"Unit \"{unitName}\" cannot be paired with itself.")
        { }
    }
}
