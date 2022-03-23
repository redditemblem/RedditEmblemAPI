using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectParameterLengthsMismatchedException : Exception
    {
        /// <summary>
        /// Thrown when an <c>ISkillEffect</c>'s parameters have lists of differing lengths.
        /// </summary>
        public SkillEffectParameterLengthsMismatchedException(params string[] parameterFieldNames)
            : base($"Skill effect parameters \"{string.Join(", ", parameterFieldNames)}\" must contain the same number of items.")
        { }
    }
}
