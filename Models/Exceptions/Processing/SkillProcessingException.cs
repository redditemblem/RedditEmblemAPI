using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class SkillProcessingException : Exception
    {
        public SkillProcessingException(string skillName, Exception innerException)
            : base(string.Format("An error occurred while processing skill \"{0}\".", skillName), innerException)
        { }
    }
}
