using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class OverrideMovementEffect : StatusConditionEffect
    {
        #region Attributes

        protected override string Name { get { return "OverrideMovement"; } }
        protected override int ParameterCount { get { return 1; } }

        public int MovementValue { get; private set; }

        #endregion

        public OverrideMovementEffect(IList<string> parameters)
            : base(parameters)
        {
            this.MovementValue = DataParser.Int_Positive(parameters, 0, "Param1");
        }
    }
}
