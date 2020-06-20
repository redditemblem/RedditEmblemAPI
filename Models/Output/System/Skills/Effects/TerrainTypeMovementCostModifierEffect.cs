﻿using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public TerrainTypeMovementCostModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 2)
                throw new SkillEffectMissingParameterException("TerrainTypeMovementCostModifier", 2, parameters.Count);

            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 1, "Param2", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //This skill has nothing to apply
        }
    }
}
