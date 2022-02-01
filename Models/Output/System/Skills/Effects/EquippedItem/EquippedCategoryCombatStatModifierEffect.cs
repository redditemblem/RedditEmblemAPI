﻿using RedditEmblemAPI.Models.Exceptions.Unmatched;
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
        private IList<string> Categories { get; set; }

        /// <summary>
        /// Param2. The unit combat stats to be affected.
        /// </summary>
        private IList<string> Stats { get; set; }

        /// <summary>
        /// Param3. The values by which to modify the <c>Stats</c>.
        /// </summary>
        private IList<int> Values { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public EquippedCategoryCombatStatModifierEffect(IList<string> parameters)
            : base(parameters)
        {
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
        /// If <paramref name="unit"/> has an item equipped with a category in <c>Categories</c>, then the values in <c>Values</c> are added as modifiers to the items in <c>Stats</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            IList<UnitInventoryItem> equippedItems = unit.Inventory.SelectMany(s => s.Items.Where(i => i != null && i.IsEquipped)).ToList();
            if (equippedItems.Count() == 0)
                return;

            //The equipped item's category must be in the category list
            if (!equippedItems.Any(ei => this.Categories.Contains(ei.Item.Category)))
                return;

            ApplyUnitCombatStatModifiers(unit, skill.Name, this.Stats, this.Values);
        }
    }
}
