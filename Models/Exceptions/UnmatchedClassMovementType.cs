using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedClassMovementType : Exception
    {
        public UnmatchedClassMovementType(string className, string movementType, ICollection<string> possibleValues)
            : base(string.Format("The movement type \"{1}\" on class \"{0}\" could not be matched to a known movement type. Possible options are {2}.", className, movementType, string.Join(", ", possibleValues)))
        { }
    }
}
