using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public abstract class SkillEffect
    {
        #region Attributes

        protected abstract string Name { get; }
        protected abstract int ParameterCount { get; }
        public SkillEffectExecutionOrder ExecutionOrder { get; protected set; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public SkillEffect(List<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new SkillEffectMissingParameterException(this.Name, this.ParameterCount, parameters.Count);

            //Set the default execution order
            this.ExecutionOrder = SkillEffectExecutionOrder.Standard;
        }

        public virtual void Apply(Unit unit, Skill skill, MapObj map, List<Unit> units)
        {
            //By default, the effect applies nothing
        }

        #region Shared Functionality Helpers

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="combatStats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitCombatStatModifiers(Unit unit, string modifierName, List<string> combatStats, List<int> modifiers)
        {
            for (int i = 0; i < combatStats.Count; i++)
            {
                string statName = combatStats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat = unit.Stats.MatchCombatStatName(statName);
                if (!stat.Modifiers.TryAdd(modifierName, value))
                    stat.Modifiers[modifierName] += value;
            }
        }

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="stats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitStatModifiers(Unit unit, string modifierName, List<string> stats, List<int> modifiers)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                string statName = stats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat = unit.Stats.MatchGeneralStatName(statName);
                if(!stat.Modifiers.TryAdd(modifierName, value))
                    stat.Modifiers[modifierName] += value;
            }
        }

        /// <summary>
        /// Helper function used by the <c>...RadiusTeleportEffect</c>s. Tests each tile in <paramref name="targetTiles"/> to ensure that <paramref name="unit"/> is capable of teleporting there, then adds valid tiles to the <paramref name="unit"/>'s movement range.
        /// </summary>
        /// <exception cref="UnmatchedMovementTypeException"></exception>
        protected void AddTeleportTargetsToUnitRange(Unit unit, List<Tile> targetTiles)
        {
            IEnumerable<TerrainTypeMovementCostSetEffect_Skill> moveCostSets_Skill = unit.GetFullSkillsList().SelectMany(s => s.Effects).OfType<TerrainTypeMovementCostSetEffect_Skill>();
            IEnumerable<TerrainTypeMovementCostSetEffect_Status> moveCostSets_Status = unit.StatusConditions.SelectMany(s => s.StatusObj.Effects).OfType<TerrainTypeMovementCostSetEffect_Status>();

            foreach (Tile tile in targetTiles)
            {
                //Ensure that this unit can move to this tile
                int moveCost;
                if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(unit.GetUnitMovementType(), out moveCost))
                    throw new UnmatchedMovementTypeException(unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys);

                //If unit is blocked from this tile, check for an effect that would allow it to access it
                if (moveCost == 99)
                {
                    TerrainTypeMovementCostSetEffect_Skill movCostSet_Skill = moveCostSets_Skill.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                    TerrainTypeMovementCostSetEffect_Status movCostSet_Status = moveCostSets_Status.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                    if (!((movCostSet_Skill != null && movCostSet_Skill.CanOverride99MoveCost) || (movCostSet_Status != null && movCostSet_Status.CanOverride99MoveCost)))
                        continue;
                }

                //Check for an enemy unit already occupying this tile
                if (tile.UnitData.Unit != null && tile.UnitData.Unit.AffiliationObj.Grouping != unit.AffiliationObj.Grouping)
                    continue;

                //If no issues arose, add the tile to the unit's movement range
                if (!unit.Ranges.Movement.Contains(tile.Coordinate))
                    unit.Ranges.Movement.Add(tile.Coordinate);
            }
        }

        #endregion
    }

    public enum SkillEffectExecutionOrder
    {
        /// <summary>
        /// The default execution order. Should be used for most normal skill effects.
        /// </summary>
        Standard,
        /// <summary>
        /// For skill effects dependent on the final value of a stat. Should NEVER modify unit stat values.
        /// </summary>
        AfterFinalStatCalculations
    }
}