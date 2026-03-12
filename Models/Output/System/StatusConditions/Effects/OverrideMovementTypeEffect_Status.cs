using RedditEmblemAPI.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    #region Interface
    
    /// <inheritdoc cref="OverrideMovementTypeEffect_Status"/>
    public interface IOverrideMovementTypeEffect_Status
    {
        /// <inheritdoc cref="OverrideMovementTypeEffect_Status.MovementType"/>
        string MovementType { get; }
    }

    #endregion Interface

    public class OverrideMovementTypeEffect_Status : StatusConditionEffect, IOverrideMovementTypeEffect_Status
    {
        #region Attributes

        protected override string Name { get { return "OverrideMovementType"; } }
        protected override int ParameterCount { get { return 1; } }

        public string MovementType { get; private set; }

        #endregion Attributes

        public OverrideMovementTypeEffect_Status(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.MovementType = DataParser.String(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }
    }
}
