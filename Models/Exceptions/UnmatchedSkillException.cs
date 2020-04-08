using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnmatchedSkillException : Exception
    {
        public UnmatchedSkillException(string skillName)
            : base(string.Format("The skill \"{0}\" could not be matched to a known skill definition.  The given name must match exactly, including capitalization and punctuation.", skillName))
        { }
    }
}
