using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class IgnoreUnitAffiliationsEffect : SkillEffect, IIgnoreUnitAffiliations
    {
        #region Attributes

        protected override string Name { get { return "IgnoreUnitAffiliations"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public IgnoreUnitAffiliationsEffect(List<string> parameters)
            : base(parameters)
        {
            //This effect has no parameters
        }

        public bool IsActive(Unit unit)
        {
            //No conditional, always true
            return true;
        }
    }
}
