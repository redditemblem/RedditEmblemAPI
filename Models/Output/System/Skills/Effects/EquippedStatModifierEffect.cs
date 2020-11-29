using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class EquippedStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. List of <c>Item</c> categories to check for.
        /// </summary>
        public IList<string> Categories { get; set; }

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
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public EquippedStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("EquippedStatModifier", 3, parameters.Count);

            this.Categories = ParseHelper.StringCSVParse(parameters, 0);
            this.Stat = ParseHelper.SafeStringParse(parameters, 1, "Param2", true);
            this.Value = ParseHelper.SafeIntParse(parameters, 2, "Param3", false);
        }

        /// <summary>
        /// If <paramref name="unit"/> has an item equipped with a category in <c>Categories</c>, then <c>Value</c> is added as a modifier of <c>Stat</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            UnitInventoryItem equipped = unit.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
            if (equipped == null)
                return;

            //The equipped item's category must be in the category list
            if (!this.Categories.Contains(equipped.Item.Category))
                return;

            ModifiedStatValue stat;
            if (!unit.Stats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
