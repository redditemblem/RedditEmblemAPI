using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class MapImageURLsNotFoundException : Exception
    {
        /// <summary>
        /// Thrown when no image URLs have been provided for the map.
        /// </summary>
        public MapImageURLsNotFoundException(string sheetName)
            : base($"Could not locate any map image URLs on sheet \"{sheetName}\".")
        { }
    }
}
