using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class SkillEffectParameterLengthsMismatchedException : Exception
    {
        /// <summary>
        /// Thrown when an <c>ISkillEffect</c>'s parameters have lists of differing lengths.
        /// </summary>
        public SkillEffectParameterLengthsMismatchedException(params string[] parameterFieldNames)
            : base(string.Format("The parameters \"{0}\" must have the same number of items.", string.Join(',', parameterFieldNames)))
        { }
    }
}
