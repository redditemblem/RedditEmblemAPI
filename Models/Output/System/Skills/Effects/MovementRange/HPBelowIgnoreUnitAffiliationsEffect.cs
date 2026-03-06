using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    #region Interface

    /// <inheritdoc cref="HPBelowIgnoreUnitAffiliationsEffect"/>
    public interface IHPBelowIgnoreUnitAffiliationsEffect : IIgnoreUnitAffiliations
    {
        /// <inheritdoc cref="HPBelowIgnoreUnitAffiliationsEffect.HPPercentage"/>
        int HPPercentage { get; }
    }

    #endregion Interface

    public class HPBelowIgnoreUnitAffiliationsEffect : SkillEffect, IHPBelowIgnoreUnitAffiliationsEffect
    {
        #region Attributes 

        protected override string Name { get { return "HPBelowIgnoreUnitAffiliations"; } }
        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The maximum HP percentage the unit can have.
        /// </summary>
        public int HPPercentage { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPBelowIgnoreUnitAffiliationsEffect(IEnumerable<string> parameters)
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
