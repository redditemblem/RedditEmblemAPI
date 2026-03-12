using RedditEmblemAPI.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    #region Interface

    /// <inheritdoc cref="TerrainTypeMovementCostSetEffect_Skill"/>
    public interface ITerrainTypeMovementCostSetEffect_Skill
    {
        /// <inheritdoc cref="TerrainTypeMovementCostSetEffect_Skill.TerrainTypeGrouping"/>
        int TerrainTypeGrouping { get; }

        /// <inheritdoc cref="TerrainTypeMovementCostSetEffect_Skill.Value"/>
        int Value { get; }

        /// <inheritdoc cref="TerrainTypeMovementCostSetEffect_Skill.CanOverride99MoveCost"/>
        bool CanOverride99MoveCost { get; }
    }

    #endregion Interface

    public class TerrainTypeMovementCostSetEffect_Skill : SkillEffect, ITerrainTypeMovementCostSetEffect_Skill
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

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public TerrainTypeMovementCostSetEffect_Skill(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Value = DataParser.Int_Positive(parameters, INDEX_PARAM_2, NAME_PARAM_2);
            this.CanOverride99MoveCost = DataParser.OptionalBoolean_YesNo(parameters, INDEX_PARAM_3, NAME_PARAM_3);
        }
    }
}
