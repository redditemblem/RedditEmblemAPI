using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class StatusConditionEffectMissingParameterException : Exception
    {
        /// <summary>
        /// Thrown when an <c>StatusConditionEffects</c>'s constructor needs more parameters than what is passed in.
        /// </summary>
        public StatusConditionEffectMissingParameterException(string statusConditionEffectName, int expectedNumberOfParms, int foundNumberOfParms)
            : base(string.Format("The status condition effect \"{0}\" requires {1} parameters to function, but only {2} were found.", statusConditionEffectName, expectedNumberOfParms, foundNumberOfParms))
        { }
    }
}