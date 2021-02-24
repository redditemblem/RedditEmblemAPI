using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitStatusConditionEffectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Unit</c>'s status condition effects.
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="innerException"></param>
        public UnitStatusConditionEffectProcessingException(string unitName, Exception innerException)
            : base("status condition effects on unit", unitName, innerException)
        { }
    }
}
