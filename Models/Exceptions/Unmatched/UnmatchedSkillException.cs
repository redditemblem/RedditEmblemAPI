namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedSkillException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>Skill</c>.
        /// </summary>
        /// <param name="skillName"></param>
        public UnmatchedSkillException(string skillName)
            : base("skill", skillName)
        { }
    }
}
