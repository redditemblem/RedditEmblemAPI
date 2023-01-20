using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class TerrainTypeMovementCostSetEffect_Skill : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "TerrainTypeMovementCostSet"; } }
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
        public TerrainTypeMovementCostSetEffect_Skill(List<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = DataParser.Int_Positive(parameters, 0, "Param1");
            this.Value = DataParser.Int_Positive(parameters, 1, "Param2");
            this.CanOverride99MoveCost = DataParser.OptionalBoolean_YesNo(parameters, 2, "Param3");
        }
    }
}
