using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class EquippedItemCombatStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. List of <c>Item</c> categories to check for.
        /// </summary>
        public IList<string> Categories { get; set; }

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
        public EquippedItemCombatStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("EquippedItemCombatStatModifier", 3, parameters.Count);

            this.Categories = ParseHelper.StringCSVParse(parameters.ElementAtOrDefault<string>(0));
            this.Stat = ParseHelper.SafeStringParse(parameters, 1, "Param2", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 2, "Param3", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            UnitInventoryItem equipped = unit.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
            if (equipped == null)
                return;

            if (!this.Categories.Contains(equipped.Item.Category))
                return;

            ModifiedStatValue stat;
            if (!unit.CombatStats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
