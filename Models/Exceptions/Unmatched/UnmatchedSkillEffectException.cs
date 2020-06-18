namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedSkillEffectException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>ISkillEffect</c> type.
        /// </summary>
        /// <param name="skillEffectName"></param>
        public UnmatchedSkillEffectException(string skillEffectName)
            : base("skill effect", skillEffectName)
        { }
    }
}
