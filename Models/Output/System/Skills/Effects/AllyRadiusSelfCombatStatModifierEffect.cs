﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class AllyRadiusSelfCombatStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The range within this skill affects units.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Param2. The unit combat stats to be affected.
        /// </summary>
        public IList<string> Stats { get; set; }

        /// <summary>
        /// Param3. The values by which to modify the <c>Stats</c>.
        /// </summary>
        public IList<int> Values { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public AllyRadiusSelfCombatStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("AllyRadiusSelfCombatStatModifier", 3, parameters.Count);

            this.Radius = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Stats = ParseHelper.StringCSVParse(parameters, 1); //Param2
            this.Values = ParseHelper.IntCSVParse(parameters, 2, "Param3", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// Searches the <paramref name="units"/> list for friendly units within <c>Radius</c> tiles. If it finds one, adds the values in <c>Values</c> as modifiers to the items in <c>Stats</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            //If unit is not on the map, don't apply
            if (unit.OriginTile == null)
                return;

            bool allyInRange = false;
            foreach (Unit other in units)
            {
                //Ignore self
                if (unit.Name == other.Name)
                    continue;

                //Units must be in the same grouping
                if (unit.AffiliationObj.Grouping != other.AffiliationObj.Grouping)
                    continue;

                //Units must be within range
                if ( other.OriginTile == null
                  || this.Radius < unit.OriginTile.Coordinate.DistanceFrom(other.OriginTile.Coordinate))
                    continue;

                //If we reach this point, we have found an allied unit in radius range
                allyInRange = true;
                break;
            }

            if (allyInRange)
            {
                for (int i = 0; i < this.Stats.Count; i++)
                {
                    string statName = this.Stats[i];
                    int value = this.Values[i];

                    ModifiedStatValue stat;
                    if (!unit.Stats.TryGetValue(statName, out stat))
                        throw new UnmatchedStatException(statName);
                    stat.Modifiers.Add(skill.Name, value);
                }
            }
        }
    }
}
