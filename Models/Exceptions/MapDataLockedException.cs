using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class MapDataLockedException : Exception
    {
        public MapDataLockedException()
            : base("The map has been locked by your GM. Please check back later!")
        { }
    }
}
