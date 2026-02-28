using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills.Effects;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges.Movement
{
    #region Interface

    /// <inheritdoc cref="Vertex"/>
    public interface IVertex
    {
        /// <inheritdoc cref="Vertex.Neighbors"/>
        IVertex[] Neighbors { get; }

        /// <inheritdoc cref="Vertex.Tiles"/>
        IEnumerable<ITile> Tiles { get; }

        /// <inheritdoc cref="Vertex.WarpEntrances"/>
        IEnumerable<ITile> WarpEntrances { get; }

        /// <inheritdoc cref="Vertex.IsTraversableOnly"/>
        public bool IsTraversableOnly { get; }

        /// <inheritdoc cref="Vertex.PathCost"/>
        public int PathCost { get; }

        /// <inheritdoc cref="Vertex.IsVisited"/>
        public bool IsVisited { get; set; }

        /// <inheritdoc cref="Vertex.MinDistanceTo"/>
        public int MinDistanceTo { get; set; }

        /// <inheritdoc cref="Vertex.IsTerminus"/>
        public bool IsTerminus { get; }

        /// <inheritdoc cref="Vertex.Reset"/>
        void Reset();

        /// <inheritdoc cref="Vertex.UpdateVertexForUnit(MovementRangeParameters, IDictionary{ITile, int})"/>
        void UpdateVertexForUnit(MovementRangeParameters parms, IDictionary<ITile, int> moveCostHistory = null);
    }

    #endregion Interface

    public class Vertex : IVertex
    {
        #region Attributes

        /// <summary>
        /// The vertices directly adjacent to this vertex in the cardinal directions.
        /// </summary>
        public IVertex[] Neighbors { get; private set; }

        /// <summary>
        /// The collection of tiles represented by this vertex.
        /// </summary>
        public IEnumerable<ITile> Tiles { get; private set; }

        public IEnumerable<ITile> WarpEntrances { get; private set; }

        /// <summary>
        /// Flag indicating that units cannot end their path on this vertex.
        /// </summary>
        public bool IsTraversableOnly { get; private set; }

        /// <summary>
        /// The amount of movement required for a unit to traverse to this vertex.
        /// </summary>
        public int PathCost { get; private set; }

        /// <summary>
        /// Flag indicating that this vertex has already been visited by the range algorithm.
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// The minimum path distance required for the unit to reach this vertex.
        /// </summary>
        public int MinDistanceTo { get; set; }

        /// <summary>
        /// Flag indicating that a unit cannot path through this vertex to any of its neighbors.
        /// </summary>
        public bool IsTerminus { get; private set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Vertex(IEnumerable<ITile> tiles)
        {
            Neighbors = new IVertex[4];
            Tiles = tiles;
            WarpEntrances = tiles.Where(t => t.TerrainType.WarpType == WarpType.Entrance || t.TerrainType.WarpType == WarpType.Dual);
            IsTraversableOnly = tiles.Any(t => t.TerrainType.CannotStopOn);

            Reset();
        }

        /// <summary>
        /// Resets unit-manipulated values on the vertex back to their default.
        /// </summary>
        public void Reset()
        {
            MinDistanceTo = int.MaxValue;
            PathCost = int.MaxValue;
            IsVisited = false;
            IsTerminus = false;
        }

        /// <summary>
        /// Updates unit-manipulated values on the vertex based on <paramref name="parms"/>.
        /// </summary>
        public void UpdateVertexForUnit(MovementRangeParameters parms, IDictionary<ITile, int> moveCostHistory)
        {
            PathCost = CalculatePathCostForUnit(parms, moveCostHistory);
            IsTerminus = IsTerminusForUnit(parms);
        }

        /// <summary>
        /// Calculates and returns the path cost for the <paramref name="parms"/> unit to traverse to this vertex.
        /// </summary>
        private int CalculatePathCostForUnit(MovementRangeParameters parms, IDictionary<ITile, int> moveCostHistory)
        {
            int maxMovCost = int.MinValue;

            foreach(ITile tile in Tiles)
            {
                //Multi-tile unit calculations have the same tile appear in multiple vertices
                //Check if we've already calculated this tile's movement cost elsewhere to save time
                int movCost;
                if (moveCostHistory is null || !moveCostHistory.TryGetValue(tile, out movCost))
                {
                    movCost = CalculateMovementCostForTile(parms, tile);

                    //Min/max value enforcement
                    movCost = Math.Max(0, movCost);
                    movCost = Math.Min(movCost, 99);

                    moveCostHistory?.Add(tile, movCost);
                }

                //For multi-tile units, a cost per tile in the vertex will be determined.
                //To ensure that the unit has enough movement to make it to *all* tiles,
                //take the highest cost out of the set.
                if(movCost > maxMovCost)
                    maxMovCost = movCost;
            }

            return maxMovCost;
        }

        /// <summary>
        /// Calculates and returns the cost for the <paramref name="parms"/> unit to traverse to the <paramref name="tile"/>.
        /// </summary>
        /// <exception cref="UnmatchedMovementTypeException"></exception>
        private int CalculateMovementCostForTile(MovementRangeParameters parms, ITile tile)
        {
            if (IsUnitBlockedFromTile(parms, tile))
            {
                //Unit is completely blocked from a tile, which means it
                //cannot access this vertex under any circumstances
                return 99;
            }

            ITerrainTypeStats terrainStats = tile.TerrainType.GetTerrainTypeStatsByAffiliation(parms.Unit.Affiliation);

            //Get the default movement cost from the terrain type
            int moveCost;
            if (!terrainStats.MovementCosts.TryGetValue(parms.Unit.GetUnitMovementType(), out moveCost))
                throw new UnmatchedMovementTypeException(parms.Unit.GetUnitMovementType(), terrainStats.MovementCosts.Keys);

            moveCost = ApplyEffectsToMovementCost(parms, tile, moveCost);

            //Check if there are nearby units affecting the movement cost for this tile
            IEnumerable<IAffectMovementCost> activeEffects = tile.UnitData.UnitsAffectingMovementCosts.SelectMany(u => u.GetFullSkillsList().SelectMany(s => s.Effects).OfType<IAffectMovementCost>().Where(e => e.IsActive(u, parms.Unit)));
            if (activeEffects.Any())
            {
                int minEffectMovCost = activeEffects.Min(e => e.GetMovementCost());
                if (minEffectMovCost < moveCost)
                    return minEffectMovCost;
            }

            return moveCost;
        }

        /// <summary>
        /// Evaluates multiple effect scenarios and returns a movement cost based on any that apply. Separated out from the other logic to allow for early returns.
        /// </summary>
        private int ApplyEffectsToMovementCost(MovementRangeParameters parms, ITile tile, int startingMovementCost)
        {
            ITerrainTypeMovementCostSetEffect_Status movCostSet_Status = parms.MoveCostSets_Statuses.FirstOrDefault(s => tile.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));
            if (movCostSet_Status is not null && (movCostSet_Status.CanOverride99MoveCost || startingMovementCost < 99))
                return movCostSet_Status.Value;

            ITerrainTypeMovementCostSetEffect_Skill movCostSet_Skill = parms.MoveCostSets_Skills.FirstOrDefault(s => tile.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));
            if (movCostSet_Skill is not null && (movCostSet_Skill.CanOverride99MoveCost || startingMovementCost < 99))
                return movCostSet_Skill.Value;

            ITerrainTypeMovementCostModifierEffect moveCostMod = parms.MoveCostModifiers.FirstOrDefault(s => tile.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));
            if (moveCostMod is not null && startingMovementCost < 99)
                return startingMovementCost + moveCostMod.Value;

            //If no effect applied, just keep our starting movement cost
            return startingMovementCost;
        }

        /// <summary>
        /// Returns true if the <paramref name="parms"/> unit is incapable of pathing through this vertex to any of its neighbors.
        /// </summary>
        public bool IsTerminusForUnit(MovementRangeParameters parms)
        {
            //Tile is considered a terminus if the unit does not ignore affilations, there are no origin tiles in the
            //vertex, and a unit is obstructing (not occupying) one of the vertex tiles.
            return !parms.IgnoresAffiliations
                 && Tiles.Any(t => !parms.Unit.Location.OriginTiles.Contains(t)
                                     && IsMovementPreventedByOtherUnit(parms.Unit, t.UnitData.UnitsObstructingMovement));
        }

        /// <summary>
        /// Returns true if the <paramref name="parms"/> unit is incapable of pathing to this tile in all scenarios.
        /// </summary>
        private bool IsUnitBlockedFromTile(MovementRangeParameters parms, ITile tile)
        {
            bool isTileUnitOrigin = parms.Unit.Location.OriginTiles.Contains(tile);

            return tile.TerrainType.RestrictAffiliations.Contains(parms.Unit.Affiliation.Grouping)
                   //The unit can potentially be blocked by other units on this tile if it does not ignore affiliations
                   ////and this isn't one of its origin tiles.
                   || !parms.IgnoresAffiliations && !isTileUnitOrigin && IsMovementPreventedByOtherUnit(parms.Unit, tile.UnitData.Unit);
        }

        /// <summary>
        /// Returns true if <paramref name="movingUnit"/> is blocked by any of the <paramref name="blockingUnits"/>.
        /// </summary>
        private bool IsMovementPreventedByOtherUnit(IUnit movingUnit, IEnumerable<IUnit> blockingUnits)
        {
            return blockingUnits.Any(u => IsMovementPreventedByOtherUnit(movingUnit, u));
        }

        /// <summary>
        /// Returns true if <paramref name="movingUnit"/> is blocked by <paramref name="blockingUnit"/>.
        /// </summary>
        private bool IsMovementPreventedByOtherUnit(IUnit movingUnit, IUnit blockingUnit)
        {
            //Check to make sure the blocking unit exists and isn't the same unit as the moving unit
            if (blockingUnit is null || movingUnit.Equals(blockingUnit))
                return false;

            //Check if the blocking unit is inflicted with a relevant status condition
            if (blockingUnit.StatusConditions.SelectMany(sc => sc.Status.Effects).OfType<DoesNotBlockEnemyAffiliationsEffect>().Any())
                return false;

            //Finally, compare the units' affiliation groupings
            return movingUnit.Affiliation.Grouping != blockingUnit.Affiliation.Grouping;
        }

        #region Static Functions

        /// <summary>
        /// Builds and returns a vertex map from <paramref name="map"/> suitable for calculating the movement of units of <paramref name="unitSize"/>
        /// </summary>
        public static IList<IVertex> BuildVertexMap(IMapObj map, int unitSize)
        {
            List<IVertex> vertexMap = new List<IVertex>();

            //Vertices cannot span more than one map segment
            //Keep tile selection contained to the current segment
            foreach (IMapSegment segment in map.Segments)
            {
                ITile[][] tiles = segment.Tiles;

                //Traverse each individual tile that could be an anchor for the unit from this segment
                IVertex[] priorRow = null;
                for (int r = 0; r < tiles.Length - unitSize + 1; r++)
                {
                    int rowVertexCount = tiles[r].Length - unitSize + 1;
                    IVertex[] currentRow = new IVertex[rowVertexCount];

                    for (int c = 0; c < rowVertexCount; c++)
                    {
                        //Grab every other tile the unit will cover if positioned here
                        List<ITile> vertexTiles = new List<ITile>();
                        for (int r2 = 0; r2 < unitSize; r2++)
                            for (int c2 = 0; c2 < unitSize; c2++)
                                vertexTiles.Add(tiles[r + r2][c + c2]);

                        IVertex vert = new Vertex(vertexTiles);
                        currentRow[c] = vert;

                        //Link the vertex to its cardinal direction neighbors
                        if (r > 0)
                        {
                            vert.Neighbors[(int)CardinalDirection.North] = priorRow[c];
                            priorRow[c].Neighbors[(int)CardinalDirection.South] = vert;
                        }
                        if (c > 0)
                        {
                            vert.Neighbors[(int)CardinalDirection.West] = currentRow[c - 1];
                            currentRow[c - 1].Neighbors[(int)CardinalDirection.East] = vert;
                        }
                    }

                    vertexMap.AddRange(currentRow);
                    priorRow = currentRow;
                }
            }

            return vertexMap;
        }

        #endregion Static Functions
    }
}