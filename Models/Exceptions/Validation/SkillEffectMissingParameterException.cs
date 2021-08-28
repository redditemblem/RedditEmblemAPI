using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectMissingParameterException : Exception
    {
        /// <summary>
        /// Thrown when an <c>ISkillEffect</c>'s constructor needs more parameters than what is passed in.
        /// </summary>
        public SkillEffectMissingParameterException(string skillEffectName, int expectedNumberOfParms, int foundNumberOfParms)
            : base($"The skill effect \"{skillEffectName}\" requires {expectedNumberOfParms} parameters to function, but only {foundNumberOfParms} were found.")
        { }

    }
}
