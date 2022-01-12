using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class WeaponRankMissingLetterException : Exception
    {
        /// <summary>
        /// Thrown when a system uses weapon ranks and a unit has a weapon rank without the letter specified.
        /// </summary>
        /// <param name="weaponRankType"></param>
        public WeaponRankMissingLetterException(string weaponRankType)
            : base($"Weapon rank \"{weaponRankType}\" is missing a rank letter.")
        { }
    }
}
