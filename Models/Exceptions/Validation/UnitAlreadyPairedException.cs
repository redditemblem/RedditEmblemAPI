using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitAlreadyPairedException : Exception
    {
        /// <summary>
        /// Thrown when <paramref name="impossibleUnitName"/> is attempting to pair with <paramref name="targetUnitName"/>, but one of them is already paired with <paramref name="pairedUnitName"/>.
        /// </summary>
        /// <param name="targetUnitName"></param>
        /// <param name="pairedUnitName"></param>
        /// <param name="impossibleUnitName"></param>
        public UnitAlreadyPairedException(string targetUnitName, string pairedUnitName, string impossibleUnitName)
            : base(string.Format("Unit \"{0}\" is already paired with unit \"{1}\". Cannot pair with unit \"{2}\".", targetUnitName, pairedUnitName, impossibleUnitName))
        { }
    }
}
