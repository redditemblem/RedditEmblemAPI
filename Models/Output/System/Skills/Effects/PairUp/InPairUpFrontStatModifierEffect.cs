using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.PairUp
{
    public class InPairUpFrontStatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "InPairUpFrontStatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1. The unit stats to be affected.
        /// </summary>
        private IList<string> Stats { get; set; }

        /// <summary>
        /// Param2. The value by which to modify the <c>Stats</c>.
        /// </summary>
        private IList<int> Values { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        /// <exception cref="SkillEffectParameterLengthsMismatchedException"></exception>
        public InPairUpFrontStatModifierEffect(IList<string> parameters)
            : base(parameters)
        {
            this.Stats = ParseHelper.StringCSVParse(parameters, 0); //Param1
            this.Values = ParseHelper.IntCSVParse(parameters, 1, "Param2", false);

            if (this.Stats.Count == 0)
                throw new RequiredValueNotProvidedException("Param1");
            if (this.Values.Count == 0)
                throw new RequiredValueNotProvidedException("Param2");

            if (this.Stats.Count != this.Values.Count)
                throw new SkillEffectParameterLengthsMismatchedException("Param1", "Param2");
        }

        /// <summary>
        /// Adds the items in <c>Values</c> as modifiers to the stats in <c>Stats</c> for <paramref name="unit"/>, if they have a paired partner.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, IList<Unit> units)
        {
            //Validate that the unit is in the front of a pairup
            if (unit.Location.PairedUnitObj == null || unit.Location.IsBackOfPair)
                return;

            ApplyUnitStatModifiers(unit, skill.Name, this.Stats, this.Values);
        }
    }
}
