using RedditEmblemAPI.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface

    /// <inheritdoc cref="OverrideMovementEffect"/>
    public interface IOverrideMovementEffect
    {
        /// <inheritdoc cref="OverrideMovementEffect.MovementValue"/>
        int MovementValue { get; }
    }

    #endregion Interface

    public class OverrideMovementEffect : StatusConditionEffect, IOverrideMovementEffect
    {
        #region Attributes

        protected override string Name { get { return "OverrideMovement"; } }
        protected override int ParameterCount { get { return 1; } }

        public int MovementValue { get; private set; }

        #endregion Attributes

        public OverrideMovementEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.MovementValue = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }
    }
}
