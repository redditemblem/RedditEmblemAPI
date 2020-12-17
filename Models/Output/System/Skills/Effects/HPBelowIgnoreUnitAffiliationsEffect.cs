using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class HPBelowIgnoreUnitAffiliationsEffect : ISkillEffect, IIgnoreUnitAffiliations
    {
        /// <summary>
        /// Param1. The maximum HP percentage the unit can have.
        /// </summary>
        public int HPPercentage { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowIgnoreUnitAffiliationsEffect(IList<string> parameters)
        {
            if (parameters.Count < 1)
                throw new SkillEffectMissingParameterException("HPBelowIgnoreUnitAffiliations", 1, parameters.Count);

            this.HPPercentage = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //This effect has nothing to apply
        }

        public bool IsActive(Unit unit)
        {
            //HP percentage must be equal to or greater than unit HP percentage
            return this.HPPercentage >= unit.HP.Percentage;
        }
    }
}
