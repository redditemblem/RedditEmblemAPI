using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats
{
    public class StatModifierEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "StatModifier"; } }
        protected override int ParameterCount { get { return 2; } }

        /// <summary>
        /// Param1/Param2. The unit stat modifiers to apply.
        /// </summary>
        private IDictionary<string, int> Modifiers { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatModifierEffect(List<string> parameters)
            : base(parameters)
        {
            this.Modifiers = DataParser.StatValueCSVs_Int_Any(parameters, INDEX_PARAM_1, NAME_PARAM_1, INDEX_PARAM_2, NAME_PARAM_2);
        }

        /// <summary>
        /// Applies <c>Modifiers</c> to <paramref name="unit"/>.
        /// </summary>
        public override void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            unit.Stats.ApplyGeneralStatModifiers(this.Modifiers, skill.Name, true);
        }
    }
}
