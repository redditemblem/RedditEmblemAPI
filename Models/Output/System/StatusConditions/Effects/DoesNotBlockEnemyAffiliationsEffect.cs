using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="DoesNotBlockEnemyAffiliationsEffect"/>
    public interface IDoesNotBlockEnemyAffiliationsEffect { }

    #endregion Interface

    public class DoesNotBlockEnemyAffiliationsEffect : StatusConditionEffect, IDoesNotBlockEnemyAffiliationsEffect
    {
        #region Attributes

        protected override string Name { get { return "DoesNotBlockEnemyAffiliations"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion Attributes

        public DoesNotBlockEnemyAffiliationsEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            //This effect has no parameters.
        }
    }
}
