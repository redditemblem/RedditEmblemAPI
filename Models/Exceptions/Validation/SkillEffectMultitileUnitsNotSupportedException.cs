using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectMultitileUnitsNotSupportedException : Exception
    {
        /// <summary>
        /// Thrown when a skill effect has not been configured to function with multi-tile units. Oops.
        /// </summary>
        public SkillEffectMultitileUnitsNotSupportedException(string skillEffectName)
            : base($"The skill effect \"{skillEffectName}\" does not currently work on multi-tile units. Please contact Iron.")
        { }
    }
}