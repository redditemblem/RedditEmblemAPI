using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public interface ISkillEffect
    {
        void Apply(Unit unit, Skill skill, IList<Unit> units);
    }
}
