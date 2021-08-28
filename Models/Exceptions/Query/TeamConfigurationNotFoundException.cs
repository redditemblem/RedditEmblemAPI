using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class TeamConfigurationNotFoundException : Exception
    {
        /// <summary>
        /// Thrown when a team configuration JSON file could not be located.
        /// </summary>
        /// <param name="fileName"></param>
        public TeamConfigurationNotFoundException(string fileName)
            : base($"Configuration for the team \"{fileName.Replace(".json", string.Empty)}\" could not be located.")
        { }
    }
}
