using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    public class HPDifferenceCombatStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "HPDifferenceCombatStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The value by which to multiply the unit's HP difference.
        /// </summary>
        private decimal Multiplier { get; set; }

        /// <summary>
        /// Param2. The unit combat stats to be affected.
        /// </summary>
        private List<string> Stats { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HPDifferenceCombatStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Multiplier = DataParser.Decimal_NonZeroPositive(parameters, 0, "Param1");
            this.Stats = DataParser.List_StringCSV(parameters, 1); //Param2
        }

        /// <summary>
        /// Calculates the HP difference and adds it as a modifier to the stats in <c>Stats</c> for <paramref name="unit"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            int modifier = (int)Math.Floor(unit.Stats.HP.Difference * this.Multiplier);
            if (modifier == 0)
                return;

            foreach (string statName in this.Stats)
            {
                ModifiedStatValue stat;
                if (!unit.Stats.Combat.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(skill.Name, modifier);
            }
        }
    }
}
