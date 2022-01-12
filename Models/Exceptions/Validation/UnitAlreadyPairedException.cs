using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitAlreadyPairedException : Exception
    {
        /// <summary>
        /// Thrown when <paramref name="impossibleUnitName"/> is attempting to pair with <paramref name="targetUnitName"/>, who is already paired with <paramref name="pairedUnitName"/>.
        /// </summary>
        /// <param name="targetUnitName"></param>
        /// <param name="pairedUnitName"></param>
        /// <param name="impossibleUnitName"></param>
        public UnitAlreadyPairedException(string targetUnitName, string pairedUnitName, string impossibleUnitName)
            : base($"Unit \"{targetUnitName}\" is already paired with unit \"{pairedUnitName}\" and cannot pair with unit \"{impossibleUnitName}\".")
        { }
    }
}
