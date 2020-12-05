using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitAlreadyPairedException : Exception
    {
        public UnitAlreadyPairedException(string targetUnitName, string pairedUnitName)
            : base(string.Format("Unit \"{0}\" is already paired with unit \"{1}\". Multiple units cannot be paired.", targetUnitName, pairedUnitName))
        { }
    }
}
