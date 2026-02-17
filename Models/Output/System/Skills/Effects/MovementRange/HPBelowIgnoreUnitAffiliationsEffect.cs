using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class HPBelowIgnoreUnitAffiliationsEffect : SkillEffect, IIgnoreUnitAffiliations
    {
        #region Attributes 

        protected override string Name { get { return "HPBelowIgnoreUnitAffiliations"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The maximum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowIgnoreUnitAffiliationsEffect(List<string> parameters)
            : base(parameters)
        {
            this.HPPercentage = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }

        public bool IsActive(IUnit unit)
        {
            //HP percentage must be equal to or greater than unit HP percentage
            return this.HPPercentage >= unit.Stats.HP.Percentage;
        }
    }
}
