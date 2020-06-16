using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedClassMovementTypeException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to a movement cost name configured in <c>TerrainTypesConfig</c>.
        /// </summary>
        /// <param name="movementType"></param>
        /// <param name="possibleValues"></param>
        public UnmatchedClassMovementTypeException(string movementType, IList<string> possibleValues)
            : base("class movement type", movementType, possibleValues)
        { }
    }
}
