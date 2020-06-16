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
            : base(string.Format("Configuration for the team \"{0}\" could not be located.", fileName.Replace(".json", string.Empty)))
        { }
    }
}
