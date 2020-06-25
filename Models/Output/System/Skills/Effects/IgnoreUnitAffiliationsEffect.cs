using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class IgnoreUnitAffiliationsEffect : ISkillEffect
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public IgnoreUnitAffiliationsEffect(IList<string> parameters)
        {
            //This effect has no parameters
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //This effect has nothing to apply
        }
    }
}
