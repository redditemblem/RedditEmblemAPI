using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem
{
    public class EquippedCategoryCombatStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "EquippedCategoryCombatStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. List of <c>Item</c> categories to check for.
        /// </summary>
        private List<string> Categories { get; set; }

        /// <summary>
        /// Param2/Param3. The unit combat stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public EquippedCategoryCombatStatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Categories = DataParser.List_StringCSV(parameters, INDEX_PARAM_1);
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_2, NAME_PARAM_2, INDEX_PARAM_3, NAME_PARAM_3);

            if (!this.Categories.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_1);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/> if <paramref name="unit"/> has an item equipped with a category in <c>Categories</c>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IReadOnlyCollection<Unit> units)
        {
            UnitInventoryItem equipped = unit.Inventory.GetPrimaryEquippedItem();
            if (equipped == null)
                return;

            //The equipped item's category must be in the category list
            if (!this.Categories.Contains(equipped.Item.Category))
                return;

            unit.Stats.ApplyCombatStatModifiers(this.Modifiers, skill.Name, true);
        }
    }
}
