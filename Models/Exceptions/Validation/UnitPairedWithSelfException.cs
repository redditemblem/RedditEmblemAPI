using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitPairedWithSelfException : Exception
    {
        /// <summary>
        /// Thrown when a unit is attempting to pair with itself.
        /// </summary>
        /// <param name="unitName"></param>
        public UnitPairedWithSelfException(string unitName)
            : base($"Unit \"{unitName}\" cannot be paired with itself.")
        { }
    }
}
