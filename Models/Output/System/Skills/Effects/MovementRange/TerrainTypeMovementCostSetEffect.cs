using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class TerrainTypeMovementCostSetEffect : SkillEffect
    {
        #region Attributes

        protected override string SkillEffectName { get { return "TerrainTypeMovementCostSet"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; private set; }

        /// <summary>
        /// Param2. The value by which to modify Mov.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Param3. Flag that indicates whether or not this skill effect can allow the unit to cross terrain it normally cannot.
        /// </summary>
        public bool CanOverride99MoveCost { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeMovementCostSetEffect(IList<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 1, "Param2", true);
            this.CanOverride99MoveCost = (ParseHelper.SafeStringParse(parameters, 2, "Param3", true) == "Yes");
        }
    }
}
