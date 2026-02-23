using RedditEmblemAPI.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class OverrideMovementTypeEffect_Skill : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "OverrideMovementType"; } }
        protected override int ParameterCount { get { return 1; } }

        public string MovementType { get; private set; }

        #endregion

        public OverrideMovementTypeEffect_Skill(List<string> parameters)
            : base(parameters)
        {
            this.MovementType = DataParser.String(parameters, INDEX_PARAM_1, NAME_PARAM_1);
        }
    }
}
