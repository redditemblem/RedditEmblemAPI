using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges
{
    public readonly struct UnitRangeParameters
    {
        public IUnit Unit { get; }

        public bool IgnoresAffiliations { get; }
        public IEnumerable<ITerrainTypeMovementCostModifierEffect> MoveCostModifiers { get; }
        public IEnumerable<ITerrainTypeMovementCostSetEffect_Skill> MoveCostSets_Skills { get; }
        public IEnumerable<ITerrainTypeMovementCostSetEffect_Status> MoveCostSets_Statuses { get; }
        public IEnumerable<IWarpMovementCostModifierEffect> WarpCostModifiers { get; }
        public IEnumerable<IWarpMovementCostSetEffect> WarpCostSets { get; }

        public UnitRangeParameters(IUnit unit)
        {
            this.Unit = unit;

            IEnumerable<ISkill> skills = unit.GetFullSkillsList();
            IEnumerable<ISkillEffect> skillEffects = skills.SelectMany(s => s.Effects);
            IEnumerable<IStatusConditionEffect> statusEffects = unit.StatusConditions.SelectMany(s => s.Status.Effects);

            IgnoresAffiliations = skillEffects.OfType<IIgnoreUnitAffiliations>().Any(e => e.IsActive(unit));
            MoveCostModifiers = skillEffects.OfType<ITerrainTypeMovementCostModifierEffect>();
            MoveCostSets_Skills = skillEffects.OfType<ITerrainTypeMovementCostSetEffect_Skill>();
            MoveCostSets_Statuses = statusEffects.OfType<ITerrainTypeMovementCostSetEffect_Status>();
            WarpCostModifiers = skillEffects.OfType<IWarpMovementCostModifierEffect>();
            WarpCostSets = skillEffects.OfType<IWarpMovementCostSetEffect>();
        }
    }
}
