using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitSkillEffectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Unit</c>'s skill effects.
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="innerException"></param>
        public UnitSkillEffectProcessingException(string unitName, Exception innerException)
            : base("skill effects on unit", unitName, innerException)
        { }
    }
}
