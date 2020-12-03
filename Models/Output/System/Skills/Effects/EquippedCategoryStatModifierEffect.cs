using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class EquippedCategoryStatModifierEffect : ISkillEffect
    {
        #region Attributes

        /// <summary>
        /// Param1. List of <c>Item</c> categories to check for.
        /// </summary>
        public IList<string> Categories { get; set; }

        /// <summary>
        /// Param2. The unit stats to be affected.
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
        public EquippedCategoryStatModifierEffect(IList<string> parameters)
        {
            if (parameters.Count < 3)
                throw new SkillEffectMissingParameterException("EquippedCategoryStatModifier", 3, parameters.Count);

            this.Categories = ParseHelper.StringCSVParse(parameters, 0);
            this.Stats = ParseHelper.StringCSVParse(parameters, 1); //Param2
            this.Values = ParseHelper.IntCSVParse(parameters, 2, "Param3", false);

            if (this.Categories.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// If <paramref name="unit"/> has an item equipped with a category in <c>Categories</c>, then the values in <c>Values</c> are added as modifiers of the items in <c>Stats</c>.
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
