using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges.Movement
{
    public class MovementRangeCalculator
    {
        #region Attributes

        private IEnumerable<IUnit> Units;
        private IMapObj Map;
        private IDictionary<int, IList<IVertex>> VertexMaps;

        public MovementRangeCalculator(IMapObj map, IEnumerable<IUnit> units)
        {
            Map = map;
            Units = units;

            VertexMaps = new Dictionary<int, IList<IVertex>>();
        }

        #endregion Attributes

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="RangeCalculationException"></exception>
        public void CalculateUnitMovementRanges()
        {
            foreach (IUnit unit in Units)
            {
                try
                {
                    //Ignore hidden units
                    if (!unit.Location.OriginTiles.Any())
                        continue;

                    int movement = CalculateUnitMovement(unit);
                    MovementRangeParameters unitParms = new MovementRangeParameters(unit);

                    IEnumerable<ICoordinate> movementRange = CalculateUnitMovementRange(unitParms, movement);

                    //Make sure we keep any coordinates inserted into the unit's range prior to the calculation
                    unit.Ranges.Movement = unit.Ranges.Movement.Union(movementRange).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    throw new RangeCalculationException(unit, ex);
                }
            }
        }

        private int CalculateUnitMovement(IUnit unit)
        {
            IModifiedStatValue movementStat = unit.Stats.MatchGeneralStatName(Map.Constants.UnitMovementStatName);
            int movementVal = movementStat.FinalValue;

            OverrideMovementEffect overrideMovEffect = unit.StatusConditions.SelectMany(s => s.Status.Effects).OfType<OverrideMovementEffect>().FirstOrDefault();
            if (overrideMovEffect != null) movementVal = overrideMovEffect.MovementValue;

            return movementVal;
        }

        private IEnumerable<ICoordinate> CalculateUnitMovementRange(MovementRangeParameters unitParms, int movementVal)
        {
            int unitSize = unitParms.Unit.Location.UnitSize;

            //Fetch the existing vertex map for this unit size or built it if needed
            bool resetVertices = true;
            IList<IVertex> vertexMap;
            if(!VertexMaps.TryGetValue(unitSize, out vertexMap))
            {
                vertexMap = Vertex.BuildVertexMap(Map, unitSize);
                VertexMaps[unitSize] = vertexMap;
                resetVertices = false; //we don't need to reset if this is a brand new vertex map
            }

            //Multi-tile unit calculations have the same tile appear in multiple vertices.
            //Store off movement costs so we don't have to keep recalculating.
            IDictionary<ITile, int> moveCostHistory = null;
            if(unitSize > 1) moveCostHistory = new Dictionary<ITile, int>();

            bool originFound = false;
            foreach(IVertex vertex in vertexMap)
            {
                if(resetVertices) vertex.Reset();

                //If this vertex contains the unit's complete origin, mark it
                if (!originFound && vertex.Tiles.Union(unitParms.Unit.Location.OriginTiles).Count() == vertex.Tiles.Count())
                {
                    vertex.MinDistanceTo = 0;
                    originFound = true;
                }

                vertex.UpdateVertexForUnit(unitParms, moveCostHistory);
            }

            TraverseVertexMap(unitParms, vertexMap);

            return vertexMap.Where(v => v.MinDistanceTo <= movementVal && v.MinDistanceTo < 99 && !v.IsTraversableOnly)
                            .SelectMany(c => c.Tiles.Select(t => t.Coordinate));
        }

        private void TraverseVertexMap(MovementRangeParameters parms, IList<IVertex> vertexMap)
        {
            //Calculate minimum possible distance to every tile based on path values
            List<ITile> warpsUsed = new List<ITile>();

            while (vertexMap.Any(c => !c.IsVisited))
            {
                IVertex vertex = vertexMap.Where(c => !c.IsVisited).MinBy(c => c.MinDistanceTo);
                vertex.IsVisited = true;

                //If this node if a terminus, do not use its path value to update its neighbors
                //If this node cannot be pathed to, skip it.
                if (vertex.IsTerminus || vertex.PathCost >= 99 || vertex.MinDistanceTo == int.MaxValue)
                    continue;

                IEnumerable<IVertex> neighbors = vertex.Neighbors.Where(n => n is not null && !n.IsVisited && n.PathCost < 99);
                foreach (IVertex neighbor in neighbors)
                    neighbor.MinDistanceTo = Math.Min(neighbor.MinDistanceTo, vertex.MinDistanceTo + neighbor.PathCost);

                //If the vertex contains a warp entrance, find the costs from its exit too.
                IEnumerable<ITile> warps = vertex.WarpEntrances.Where(w => !warpsUsed.Contains(w));
                foreach(ITile entrance in warps)
                {
                    warpsUsed.Add(entrance);
                    int warpCost = CalculateTileWarpMovementCost(parms, entrance);

                    bool warpUpdated = false;
                    foreach (ITile exit in entrance.WarpData.WarpGroup.Where(t => entrance.Coordinate != t.Coordinate && (t.TerrainType.WarpType == WarpType.Exit || t.TerrainType.WarpType == WarpType.Dual)))
                    {
                        IEnumerable<IVertex> exitVertices = vertexMap.Where(v => v.Tiles.Contains(exit));
                        foreach (IVertex exitVertex in exitVertices)
                        {
                            //Ignore any exit on an untraversable vertex
                            if (exitVertex.PathCost >= 99)
                                continue;

                            if (vertex.MinDistanceTo + warpCost < exitVertex.MinDistanceTo)
                            {
                                exitVertex.MinDistanceTo = vertex.MinDistanceTo + warpCost;
                                warpUpdated = true;
                            }
                        }
                    }

                    //Reset visited status of all vertices
                    if (warpUpdated) 
                        foreach(IVertex vert in vertexMap)
                            vert.IsVisited = false;
                }
            }
        }

 
        private int CalculateTileWarpMovementCost(MovementRangeParameters parms, ITile warp)
        {
            IWarpMovementCostSetEffect warpCostSet = parms.WarpCostSets.FirstOrDefault(s => warp.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));
            IWarpMovementCostModifierEffect warpCostMod = parms.WarpCostModifiers.FirstOrDefault(s => warp.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));

            int warpCost = warp.TerrainType.WarpCost;
            if (warpCostSet is not null) warpCost = warpCostSet.Value;
            else if (warpCostMod is not null) warpCost += warpCostMod.Value;

            warpCost = Math.Max(0, warpCost); //enforce minimum

            return warpCost;
        }
    }
}