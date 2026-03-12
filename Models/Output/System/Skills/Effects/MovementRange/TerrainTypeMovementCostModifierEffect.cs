using RedditEmblemAPI.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    #region Interface

    /// <inheritdoc cref="TerrainTypeMovementCostModifierEffect"/>
    public interface ITerrainTypeMovementCostModifierEffect
    {
        /// <inheritdoc cref="TerrainTypeMovementCostModifierEffect.TerrainTypeGrouping"/>
        int TerrainTypeGrouping { get; }

        /// <inheritdoc cref="TerrainTypeMovementCostModifierEffect.Value"/>
        int Value { get; }
    }

    #endregion Interface

    public class TerrainTypeMovementCostModifierEffect : SkillEffect, ITerrainTypeMovementCostModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "TerrainTypeMovementCostModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; private set; }

        /// <summary>
        /// Param2. The value by which to modify Mov.
        /// </summary>
        public int Value { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeMovementCostModifierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Value = DataParser.Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2);
        }
    }
}
