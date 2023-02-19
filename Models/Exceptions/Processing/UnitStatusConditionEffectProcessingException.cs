using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitStatusConditionEffectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Unit</c>'s status condition effects.
        /// </summary>
        public UnitStatusConditionEffectProcessingException(string unitName, string statusConditionName, Exception innerException)
            : base($"effects on status condition \"{statusConditionName}\" on unit", unitName, innerException)
        { }
    }
}
