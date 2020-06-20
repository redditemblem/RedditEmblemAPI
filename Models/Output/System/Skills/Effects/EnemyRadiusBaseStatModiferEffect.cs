﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class EnemyRadiusBaseStatModiferEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Param2. The unit combat stat to be affected.
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
        public EnemyRadiusBaseStatModiferEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("EnemyRadiusBaseStatModifer", 3, parameters.Count);

            this.Radius = ParseHelper.SafeIntParse(parameters.ElementAtOrDefault<string>(0), "Param1", true);
            this.Stat = ParseHelper.SafeStringParse(parameters, 1, "Param2", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 2, "Param3", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            foreach (Unit other in units)
            {
                if (unit.Name == other.Name)
                    continue;

                //Units must be in different groupings
                if (unit.AffiliationObj.Grouping == other.AffiliationObj.Grouping)
                    continue;

                //Units must be within range
                if (this.Radius < unit.Coordinate.DistanceFrom(other.Coordinate))
                    continue;

                ModifiedStatValue stat;
                if (!other.Stats.TryGetValue(this.Stat, out stat))
                    throw new UnmatchedStatException(this.Stat);
                stat.Modifiers.Add(unit.Name + "'s " + skill.Name, this.Value);
            }
        }
    }
}
