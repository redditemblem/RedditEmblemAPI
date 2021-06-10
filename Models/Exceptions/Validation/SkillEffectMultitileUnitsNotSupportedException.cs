using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectMultitileUnitsNotSupportedException : Exception
    {
        /// <summary>
        /// Thrown when a numerical value fails to parse and the expected result can be positive or negative.
        /// </summary>
        public SkillEffectMultitileUnitsNotSupportedException(string skillEffectName)
            : base($"The skill effect \"{skillEffectName}\" does not currently work on multi-tile units. Please contact Iron.")
        { }
    }
}