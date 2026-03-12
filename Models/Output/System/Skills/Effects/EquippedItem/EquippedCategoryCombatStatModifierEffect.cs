using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.EquippedItem
{
    #region Interface

    /// <inheritdoc cref="EquippedCategoryCombatStatModifierEffect"/>
    public interface IEquippedCategoryCombatStatModifierEffect
    {
        /// <inheritdoc cref="EquippedCategoryCombatStatModifierEffect.Categories"/>
        IEnumerable<string> Categories { get; }

        /// <inheritdoc cref="EquippedCategoryCombatStatModifierEffect.Modifiers"/>
        IDictionary<string, int> Modifiers { get; }
    }

    #endregion Interface

    public class EquippedCategoryCombatStatModifierEffect : SkillEffect, IEquippedCategoryCombatStatModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "EquippedCategoryCombatStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. List of <c>Item</c> categories to check for.
        /// </summary>
        public IEnumerable<string> Categories { get; private set; }

        /// <summary>
        /// Param2/Param3. The unit combat stat modifiers to apply.
        /// </summary>
        public IDictionary<string, int> Modifiers { get; private set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public EquippedCategoryCombatStatModifierEffect(IEnumerable<string> parameters)
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
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            IUnitInventoryItem equipped = unit.Inventory.GetPrimaryEquippedItem();
            if (equipped is null) return;

            //The equipped item's category must be in the category list
            if (!this.Categories.Contains(equipped.Item.Category))
                return;

            unit.Stats.ApplyCombatStatModifiers(this.Modifiers, skill.Name, true);
        }
    }
}
