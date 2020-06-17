using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class FlatUnitStatModiferEffect : ISkillEffect
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

        public FlatUnitStatModiferEffect(string param1, string param2)
        {
            this.Stat = param1;
            this.Value = ParseHelper.SafeIntParse(param2, "Param2", false);
        }

        public void Apply(Unit unit, Skill skill)
        {
            ModifiedStatValue stat;
            if (!unit.Stats.TryGetValue(this.Stat, out stat))
                throw new Exception("Not written!");
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
