using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    #region Interface

    public interface IIgnoreUnitAffiliations
    {
        bool IsActive(IUnit unit);
    }

    #endregion Interface

    public class IgnoreUnitAffiliationsEffect : SkillEffect, IIgnoreUnitAffiliations
    {
        #region Attributes

        protected override string Name { get { return "IgnoreUnitAffiliations"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public IgnoreUnitAffiliationsEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            //This effect has no parameters
        }

        public bool IsActive(IUnit unit)
        {
            //No conditional, always true
            return true;
        }
    }
}
