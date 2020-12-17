using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class IgnoreUnitAffiliationsEffect : ISkillEffect, IIgnoreUnitAffiliations
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

        public bool IsActive(Unit unit)
        {
            //No conditional, always true
            return true;
        }
    }
}
