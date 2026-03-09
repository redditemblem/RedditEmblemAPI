using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
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
        /// Calculates the movement ranges for all units on the map.
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

                    int movement = GetUnitMovementFinalValue(unit);
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

        /// <summary>
        /// Returns <paramref name="unit"/>'s final movement stat value.
        /// </summary>
        private int GetUnitMovementFinalValue(IUnit unit)
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
            while (vertexMap.Any(c => !c.IsVisited))
            {
                IVertex vertex = vertexMap.Where(c => !c.IsVisited).MinBy(c => c.MinDistanceTo);
                vertex.IsVisited = true;

                //If this node if a terminus, do not use its path value to update its neighbors
                //If this node cannot be pathed to, skip it.
                if (vertex.IsTerminus || vertex.PathCost >= 99 || vertex.MinDistanceTo == int.MaxValue)
                    continue;

                //Update path distances for neighboring vertices that the unit can move to from here
                IEnumerable<IVertex> neighbors = vertex.Neighbors.Where(n => n is not null && !n.IsVisited && n.PathCost < 99);
                foreach (IVertex neighbor in neighbors)
                    neighbor.MinDistanceTo = Math.Min(neighbor.MinDistanceTo, vertex.MinDistanceTo + neighbor.PathCost);

                foreach(IVertexWarp warp in vertex.WarpNeighbors)
                {
                    foreach (IVertex warpNeighbor in warp.Neighbors.Where(n => !n.IsVisited && n.PathCost < 99))
                        warpNeighbor.MinDistanceTo = Math.Min(warpNeighbor.MinDistanceTo, vertex.MinDistanceTo + warp.WarpCost);
                }
            }
        }
    }
}