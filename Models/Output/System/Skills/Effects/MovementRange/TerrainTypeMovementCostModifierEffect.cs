using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class TerrainTypeMovementCostModifierEffect : SkillEffect
    {
        #region Attributes
        protected override string Name { get { return "TerrainTypeMovementCostModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; set; }

        /// <summary>
        /// Param2. The value by which to modify Mov.
        /// </summary>
        public int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeMovementCostModifierEffect(IList<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = ParseHelper.Int_Positive(parameters, 0, "Param1");
            this.Value = ParseHelper.Int_Any(parameters, 1, "Param2");
        }
    }
}
