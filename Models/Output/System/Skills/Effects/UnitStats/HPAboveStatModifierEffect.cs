using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    public class HPAboveStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string SkillEffectName { get { return "HPAboveStatModifier"; } }
        protected override int ParameterCount { get { return 3; } }

        /// <summary>
        /// Param1. The minimum HP percentage the unit can have.
        /// </summary>
        private int HPPercentage { get; set; }

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
        public HPAboveStatModifierEffect(IList<string> parameters)
            : base(parameters)
        {
            this.HPPercentage = ParseHelper.SafeIntParse(parameters, 0, "Param1", true);
            this.Stats = ParseHelper.StringCSVParse(parameters, 1); //Param2
            this.Values = ParseHelper.IntCSVParse(parameters, 2, "Param3", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param3");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param2", "Param3");
        }

        /// <summary>
        /// If <paramref name="unit"/>'s HP percentage is equal to or above the value of <c>HPPercentage</c>, adds the values in <c>Values</c> as modifiers to the items in <c>Stats</c>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //HP percentage must be equal to or above threshold
            if (unit.HP.Percentage < this.HPPercentage)
                return;

            ApplyUnitStatModifiers(unit, skill.Name, this.Stats, this.Values);
        }
    }
}
