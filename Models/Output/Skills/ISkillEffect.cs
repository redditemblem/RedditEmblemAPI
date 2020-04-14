namespace RedditEmblemAPI.Models.Output.Skills
{
    public interface ISkillEffect
    {
        void Apply(Unit unit, Skill skill);
    }
}
