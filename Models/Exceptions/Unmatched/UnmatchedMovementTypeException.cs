using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedMovementTypeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to a movement cost name configured in <c>TerrainTypesConfig</c>.
        /// </summary>
        public UnmatchedMovementTypeException(string movementType, IEnumerable<string> possibleValues)
            : base("movement type", movementType, possibleValues)
        { }
    }
}
