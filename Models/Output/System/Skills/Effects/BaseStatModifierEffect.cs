using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class BaseStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. The unit base stat to be affected.
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
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public BaseStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 2)
                throw new SkillEffectMissingParameterException("BaseStatModifier", 2, parameters.Count);

            this.Stat = ParseHelper.SafeStringParse(parameters, 0, "Param1", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 1, "Param2", false);
        }

        /// <summary>
        /// Adds <c>Value</c> as a modifier to <c>Stat</c> for <paramref name="unit"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            ModifiedStatValue stat;
            if (!unit.Stats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
