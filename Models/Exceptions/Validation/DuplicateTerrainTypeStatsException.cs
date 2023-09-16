using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class DuplicateTerrainTypeStatsException : Exception
    {
        /// <summary>
        /// Thrown when more than one <c>TerrainTypeStats</c> possesses the same affiliation grouping.
        /// </summary>
        public DuplicateTerrainTypeStatsException(int duplicateAffiliationGrouping)
            : base($"Affiliation grouping \"{duplicateAffiliationGrouping}\" cannot appear more than once in the terrain type's stat sets.")
        { }
    }
}