using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class PreventMovementEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "PreventMovement"; } }
        protected override int ParameterCount { get { return 0; } }

        #endregion

        public PreventMovementEffect(IList<string> parameters)
            : base(parameters)
        {
            //This effect has no additional parameters.
        }
    }
}
