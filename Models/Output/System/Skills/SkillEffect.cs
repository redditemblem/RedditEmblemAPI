using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public abstract class SkillEffect
    {
        #region Attributes

        protected abstract string SkillEffectName { get; }
        protected abstract int ParameterCount { get; }

        #endregion

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <exception cref="SkillEffectMissingParameterException"></exception>
        public SkillEffect(IList<string> parameters)
        {
            //Make sure enough parameters were passed in
            if (parameters.Count < this.ParameterCount)
                throw new SkillEffectMissingParameterException(this.SkillEffectName, this.ParameterCount, parameters.Count);
        }

        public virtual void Apply(Unit unit, Skill skill, MapObj Map, IList<Unit> units)
        {
            //By default, the effect applies nothing
        }

        #region Shared Functionality Helpers

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="combatStats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitCombatStatModifiers(Unit unit, string modifierName, IList<string> combatStats, IList<int> modifiers)
        {
            for (int i = 0; i < combatStats.Count; i++)
            {
                string statName = combatStats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat;
                if (!unit.CombatStats.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(modifierName, value);
            }
        }

        /// <summary>
        /// Helper function. Applies the values in <paramref name="modifiers"/> to the stats in <paramref name="stats"/>. Assumes both lists are the same length.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        protected void ApplyUnitStatModifiers(Unit unit, string modifierName, IList<string> stats, IList<int> modifiers)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                string statName = stats[i];
                int value = modifiers[i];

                if (value == 0) continue;

                ModifiedStatValue stat;
                if (!unit.Stats.TryGetValue(statName, out stat))
                    throw new UnmatchedStatException(statName);
                stat.Modifiers.Add(modifierName, value);
            }
        }

        /// <summary>
        /// Helper function used by the <c>...RadiusTeleportEffect</c>s. Tests each tile in <paramref name="targetTiles"/> to ensure that <paramref name="unit"/> is capable of teleporting there, then adds valid tiles to the <paramref name="unit"/>'s movement range.
        /// </summary>
        /// <exception cref="UnmatchedMovementTypeException"></exception>
        protected void AddTeleportTargetsToUnitRange(Unit unit, IList<Tile> targetTiles)
        {
            IList<TerrainTypeMovementCostSetEffect> moveCostSets = unit.SkillList.Select(s => s.Effect).OfType<TerrainTypeMovementCostSetEffect>().ToList();

            foreach (Tile tile in targetTiles)
            {
                //Ensure that this unit can move to this tile
                int moveCost;
                if (!tile.TerrainTypeObj.MovementCosts.TryGetValue(unit.GetUnitMovementType(), out moveCost))
                    throw new UnmatchedMovementTypeException(unit.GetUnitMovementType(), tile.TerrainTypeObj.MovementCosts.Keys.ToList());

                //If unit is blocked from this tile, check for a skill that would allow it to access it
                if (moveCost == 99)
                {
                    TerrainTypeMovementCostSetEffect movCostSet = moveCostSets.FirstOrDefault(s => tile.TerrainTypeObj.Groupings.Contains(s.TerrainTypeGrouping));
                    if (movCostSet == null || !movCostSet.CanOverride99MoveCost)
                        continue;
                }

                //Check for an enemy unit already occupying this tile
                if (tile.Unit != null && tile.Unit.AffiliationObj.Grouping != unit.AffiliationObj.Grouping)
                    continue;

                //If no issues arose, add the tile to the unit's movement range
                unit.MovementRange.Add(tile.Coordinate);
            }
        }

        #endregion
    }
}
