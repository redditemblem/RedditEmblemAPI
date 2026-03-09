using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    #region Interface

    /// <inheritdoc cref="HPDifferenceStatModifierEffect"/>
    public interface IHPDifferenceStatModifierEffect
    {
        /// <inheritdoc cref="HPDifferenceStatModifierEffect.Multiplier"/>
        decimal Multiplier { get; }

        /// <inheritdoc cref="HPDifferenceStatModifierEffect.Stats"/>
        IEnumerable<string> Stats { get; }
    }

    #endregion Interface

    public class HPDifferenceStatModifierEffect : SkillEffect, IHPDifferenceStatModifierEffect
    {
        #region Attributes

        protected override string Name { get { return "HPDifferenceStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The value by which to multiply the unit's HP difference.
        /// </summary>
        public decimal Multiplier { get; private set; }

        /// <summary>
        /// Param2. The unit stats to be affected.
        /// </summary>
        public IEnumerable<string> Stats { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public HPDifferenceStatModifierEffect(IEnumerable<string> parameters)
            : base(parameters)
        {
            this.Multiplier = DataParser.Decimal_NonZeroPositive(parameters, INDEX_PARAM_1, NAME_PARAM_1);
            this.Stats = DataParser.List_StringCSV(parameters, INDEX_PARAM_2);

            if (!this.Stats.Any())
                throw new RequiredValueNotProvidedException(NAME_PARAM_2);
        }

        /// <summary>
        /// Calculates the HP difference and adds it as a modifier to the stats in <c>Stats</c> for <paramref name="unit"/>.
        /// </summary>
        public override void Apply(IUnit unit, ISkill skill, IMapObj map, List<IUnit> units)
        {
            int modifier = (int)Math.Floor(unit.Stats.HP.Difference * this.Multiplier);
            if (modifier == 0) return;

            IDictionary<string, int> modifiers = this.Stats.Select(stat => new { stat, modifier }).ToDictionary(m => m.stat, m => m.modifier);
            unit.Stats.ApplyGeneralStatModifiers(modifiers, skill.Name, true);
        }
    }
}
