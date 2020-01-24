using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class TeamConfigurationNotFoundException : Exception
    {
        public TeamConfigurationNotFoundException(string fileName)
            : base(string.Format("Configuration for the team \"{0}\" could not be located.", fileName.Replace(".json", string.Empty)))
        { }
    }
}
