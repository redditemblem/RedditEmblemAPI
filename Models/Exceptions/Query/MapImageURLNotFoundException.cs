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
            : base(string.Format("The map image URL located on sheet \"{0}\" was found to be blank.", sheetName))
        { }
    }
}
