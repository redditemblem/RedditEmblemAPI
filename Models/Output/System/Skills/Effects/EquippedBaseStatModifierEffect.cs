using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public class EquippedBaseStatModifierEffect : ISkillEffect
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

        public EquippedBaseStatModifierEffect(string param1, string param2, string param3)
        {
            this.Categories = ParseHelper.StringCSVParse(param1);
            this.Stat = param2;
            this.Value = ParseHelper.SafeIntParse(param3, "Param3", false);
        }

        public void Apply(Unit unit, Skill skill, IList<Unit> units)
        {
            UnitInventoryItem equipped = unit.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
            if (equipped == null)
                return;

            if (!this.Categories.Contains(equipped.Item.Category))
                return;

            ModifiedStatValue stat;
            if (!unit.Stats.TryGetValue(this.Stat, out stat))
                throw new UnmatchedStatException(this.Stat);
            stat.Modifiers.Add(skill.Name, this.Value);
        }
    }
}
