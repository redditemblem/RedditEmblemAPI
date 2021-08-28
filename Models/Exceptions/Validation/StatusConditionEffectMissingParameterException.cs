using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class StatusConditionEffectMissingParameterException : Exception
    {
        /// <summary>
        /// Thrown when an <c>StatusConditionEffects</c>'s constructor needs more parameters than what is passed in.
        /// </summary>
        public StatusConditionEffectMissingParameterException(string statusConditionEffectName, int expectedNumberOfParms, int foundNumberOfParms)
            : base($"The status condition effect \"{statusConditionEffectName}\" requires {expectedNumberOfParms} parameters to function, but only {foundNumberOfParms} were found.")
        { }
    }
}