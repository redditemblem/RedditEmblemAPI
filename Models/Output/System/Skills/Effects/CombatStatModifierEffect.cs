using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public CombatStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 2)
                throw new SkillEffectMissingParameterException("CombatStatModifier", 2, parameters.Count);

            this.Stat = parameters.ElementAt<string>(0);
            this.Value = ParseHelper.SafeIntParse(parameters.ElementAt<string>(1), "Param2", false);
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
