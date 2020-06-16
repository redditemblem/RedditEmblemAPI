using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class MapDataLockedException : Exception
    {
        /// <summary>
        /// Thrown when the On/Off switch on a map is set to "Off".
        /// </summary>
        public MapDataLockedException()
            : base("The map has been locked by your GM. Please check back later!")
        { }
    }
}
