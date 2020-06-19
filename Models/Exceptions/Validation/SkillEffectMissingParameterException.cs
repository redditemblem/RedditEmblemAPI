using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectMissingParameterException : Exception
    {
        /// <summary>
        /// Thrown when an <c>ISkillEffect</c>'s constructor needs more parameters than what is passed in.
        /// </summary>
        public SkillEffectMissingParameterException(string skillEffectName, int expectedNumberOfParms, int foundNumberOfParms)
            : base(string.Format("The skill effect \"{0}\" requires {1} parameters to function, but only {2} were found.", skillEffectName, expectedNumberOfParms, foundNumberOfParms))
        { }

    }
}
