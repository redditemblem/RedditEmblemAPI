using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class WarpMovementCostSetEffect : SkillEffect
    {
        #region Attributes

        protected override string SkillEffectName { get { return "WarpMovementCostSet"; } }
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
        public WarpMovementCostSetEffect(IList<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 1, "Param2", false);
        }
    }
}
