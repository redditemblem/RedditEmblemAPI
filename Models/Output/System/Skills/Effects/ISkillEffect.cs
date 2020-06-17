using RedditEmblemAPI.Models.Output.Units;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public interface ISkillEffect
    {
        void Apply(Unit unit, Skill skill);
    }
}
