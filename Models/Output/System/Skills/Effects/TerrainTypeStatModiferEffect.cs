﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class TerrainTypeStatModiferEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The terrain type grouping to look for <c>Tile</c>s in.
        /// </summary>
        public int TerrainTypeGrouping { get; set; }

        /// <summary>
        /// Param2. The unit base stat to be affected.
        /// </summary>
        public string Stat { get; set; }

        /// <summary>
        /// Param3. The value by which to modify the <c>Stat</c>.
        /// </summary>
        public int Value { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public TerrainTypeStatModiferEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("TerrainTypeStatModifer", 3, parameters.Count);

            this.TerrainTypeGrouping = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Stat = ParseHelper.SafeStringParse(parameters, 1, "Param2", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 2, "Param3", false);
        }

        /// <summary>
        /// If <paramref name="unit"/> originates on a tile with a terrain type in <c>TerrainTypeGrouping</c>, then <c>Value</c> is added as a modifier on <c>Stat</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //The terrain type must be in the defined grouping
            if (!unit.OriginTile.TerrainTypeObj.Groupings.Contains(this.TerrainTypeGrouping))
                return;

            ModifiedStatValue stat;
            if (!unit.Stats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}