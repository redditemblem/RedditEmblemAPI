using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class HPBelowIgnoreUnitAffiliationsEffect : SkillEffect, IIgnoreUnitAffiliations
    {
        #region Attributes 

        protected override string SkillEffectName { get { return "HPBelowIgnoreUnitAffiliations"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The maximum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowIgnoreUnitAffiliationsEffect(IList<string> parameters)
            : base(parameters)
        {
            this.HPPercentage = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
        }

        public bool IsActive(Unit unit)
        {
            //HP percentage must be equal to or greater than unit HP percentage
            return this.HPPercentage >= unit.HP.Percentage;
        }
    }
}
