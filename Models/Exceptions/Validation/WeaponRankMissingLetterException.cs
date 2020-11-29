using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class WeaponRankMissingLetterException : Exception
    {
        public WeaponRankMissingLetterException(string weaponRankType)
            : base(string.Format("Weapon rank \"{0}\" is missing a rank letter.", weaponRankType))
        { }
    }
}
