using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class TerrainTypeMovementCostSetEffect : ISkillEffect
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

        /// <summary>
        /// Param3. Flag that indicates whether or not this skill effect can allow the unit to cross terrain it normally cannot.
        /// </summary>
        public bool CanOverride99MoveCost { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public TerrainTypeMovementCostSetEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("TerrainTypeMovementCostSet", 3, parameters.Count);

            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 1, "Param2", true);
            this.CanOverride99MoveCost = (ParseHelper.SafeStringParse(parameters, 2, "Param3", true) == "Yes");
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //This skill has nothing to apply
        }
    }
}
