using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class SkillProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>Skill</c>.
        /// </summary>
        /// <param name="skillName"></param>
        /// <param name="innerException"></param>
        public SkillProcessingException(string skillName, Exception innerException)
            : base("skill", skillName, innerException)
        { }
    }
}
