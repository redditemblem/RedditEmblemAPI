using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class TerrainTypeMovementCostModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; set; }

        /// <summary>
        /// Param2. The value by which to modify Mov.
        /// </summary>
        public int Value { get; set; }

        #endregion

        public TerrainTypeMovementCostModifierEffect(string param1, string param2)
        {
            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(param1, "Param1", false);
            this.Value = ParseHelper.SafeIntParse(param2, "Param2", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //This skill has nothing to apply
        }
    }
}
