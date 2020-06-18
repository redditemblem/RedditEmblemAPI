using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class CombatStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The unit combat stat to be affected.
        /// </summary>
        public string Stat { get; private set; }

        /// <summary>
        /// Param2. The value by which to modify the <c>Stat</c>.
        /// </summary>
        public int Value { get; private set; }

        #endregion

        public CombatStatModifierEffect(string param1, string param2)
        {
            this.Stat = param1;
            this.Value = ParseHelper.SafeIntParse(param2, "Param2", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            ModifiedStatValue stat;
            if (!unit.CombatStats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
