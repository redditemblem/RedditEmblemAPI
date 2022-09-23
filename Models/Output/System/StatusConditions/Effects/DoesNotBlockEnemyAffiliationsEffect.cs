using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class DoesNotBlockEnemyAffiliationsEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "DoesNotBlockEnemyAffiliations"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        public DoesNotBlockEnemyAffiliationsEffect(List<string> parameters)
            : base(parameters)
        {
            //This effect has no parameters.
        }
    }
}
