using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class MapImageURLNotFoundException : Exception
    {
        /// <summary>
        /// Thrown when the provided URL for a map image is blank.
        /// </summary>
        /// <param name="sheetName"></param>
        public MapImageURLNotFoundException(string sheetName)
            : base($"The map image URL located on sheet \"{sheetName}\" was found to be blank.")
        { }
    }
}
