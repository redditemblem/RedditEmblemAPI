using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.StatusConditions
{
    public abstract class StatusConditionEffect
    {
        #region Attributes

        protected abstract string Name { get; }
        protected abstract int ParameterCount { get; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public StatusConditionEffect(IList<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new StatusConditionEffectMissingParameterException(this.Name, this.ParameterCount, parameters.Count);
        }

        public virtual void Apply(Unit unit, StatusCondition status)
        {
            //By default, the effect applies nothing
        }

        #region Shared Functionality Helpers

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="combatStats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitCombatStatModifiers(Unit unit, string modifierName, IList<string> combatStats, IList<int> modifiers)
        {
            for (int i = 0; i < combatStats.Count; i++)
            {
                string statName = combatStats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat;
                if (!unit.CombatStats.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(modifierName, value);
            }
        }

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="stats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitStatModifiers(Unit unit, string modifierName, IList<string> stats, IList<int> modifiers)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                string statName = stats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat;
                if (!unit.Stats.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(modifierName, value);
            }
        }

        #endregion
    }
}
