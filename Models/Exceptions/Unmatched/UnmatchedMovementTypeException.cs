using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedMovementTypeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to a movement cost name configured in <c>TerrainTypesConfig</c>.
        /// </summary>
        /// <param name="movementType"></param>
        /// <param name="possibleValues"></param>
        public UnmatchedMovementTypeException(string movementType, List<string> possibleValues)
            : base("movement type", movementType, possibleValues)
        { }
    }
}
