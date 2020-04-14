using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedMovementTypeException : Exception
    {
        public UnmatchedMovementTypeException(string movementType, ICollection<string> options)
            : base(string.Format("Movement type \"{0}\" is not a known value. Configured options are {1}.", movementType, string.Join(", ", options)))
        { }
    }
}
