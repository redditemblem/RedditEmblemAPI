using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class UnitSkillEffectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Unit</c>'s skill effects.
        /// </summary>
        public UnitSkillEffectProcessingException(string unitName, string skillName, Exception innerException)
            : base($"effects on skill \"{skillName}\" on unit", unitName, innerException)
        { }
    }
}
