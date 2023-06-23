using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions.Effects
{
    public class TerrainTypeMovementCostSetEffect_Status : StatusConditionEffect
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
        public TerrainTypeMovementCostSetEffect_Status(List<string> parameters)
            : base(parameters)
        {
            this.TerrainTypeGrouping = DataParser.Int_Positive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Value = DataParser.Int_Positive(parameters, INDEX_PARAM_2, NAME_PARAM_2);
            this.CanOverride99MoveCost = DataParser.OptionalBoolean_YesNo(parameters, INDEX_PARAM_3, NAME_PARAM_3);
        }
    }
}
