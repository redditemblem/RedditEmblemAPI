using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange
{
    public class OriginAllyMovementCostSetEffect : SkillEffect
    {
        #region Attributes

        protected override string Name { get { return "OriginAllyMovementCostSet"; } }

        protected override int ParameterCount { get { return 1; } }

        /// <summary>
        /// Param1. The movement cost for allies to move through tiles occupied by a unit with this skill.
        /// </summary>
        public int MovementCost { get; set; }

        #endregion

        public OriginAllyMovementCostSetEffect(IList<string> parameters)
            : base(parameters)
        {
            this.MovementCost = ParseHelper.Int_Positive(parameters, 0, "Param1");
        }
    }
}
