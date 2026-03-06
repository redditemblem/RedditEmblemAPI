using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.MovementRange;
using System;
using System.Linq;

namespace RedditEmblemAPI.Helpers.Ranges.Movement
{
    #region Interface

    /// <inheritdoc cref="VertexWarp"/>
    public interface IVertexWarp
    {
        /// <inheritdoc cref="VertexWarp.WarpEntrance"/>
        ITile WarpEntrance { get; }

        /// <inheritdoc cref="VertexWarp.Neighbors"/>
        IVertex[] Neighbors { get; }

        /// <inheritdoc cref="VertexWarp.WarpCost"/>
        int WarpCost { get; }

        /// <inheritdoc cref="VertexWarp.Reset"/>
        void Reset();

        /// <inheritdoc cref="VertexWarp.UpdateVertexWarpForUnit(MovementRangeParameters)"/>
        void UpdateVertexWarpForUnit(MovementRangeParameters parms);
    }

    #endregion Interface

    public class VertexWarp : IVertexWarp
    {
        #region Attributes

        /// <summary>
        /// The warp entrance tile.
        /// </summary>
        public ITile WarpEntrance { get; private set; }

        /// <summary>
        /// The set of vertices that this warp entrance connects to.
        /// </summary>
        public IVertex[] Neighbors { get; private set; }

        /// <summary>
        /// The amount of movement required for a unit to use this warp entrance.
        /// </summary>
        public int WarpCost { get; private set; }

        #endregion Attributes

        public VertexWarp(ITile warpEntrance, IVertex[] neighbors)
        {
            WarpEntrance = warpEntrance;
            Neighbors = neighbors;

            Reset();
        }

        /// <summary>
        /// Resets unit-manipulated values on the warp back to their default.
        /// </summary>
        public void Reset()
        {
            WarpCost = int.MaxValue;
        }

        /// <summary>
        /// Updates unit-manipulated values on the warp based on <paramref name="parms"/>.
        /// </summary>
        public void UpdateVertexWarpForUnit(MovementRangeParameters parms)
        {
            WarpCost = CalculateWarpCostForUnit(parms);
        }

        /// <summary>
        /// Calculates and returns the warp cost for the <paramref name="parms"/> unit to use this warp entrance tile.
        /// </summary>
        private int CalculateWarpCostForUnit(MovementRangeParameters parms)
        {
            IWarpMovementCostSetEffect warpCostSet = parms.WarpCostSets.FirstOrDefault(s => WarpEntrance.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));
            IWarpMovementCostModifierEffect warpCostMod = parms.WarpCostModifiers.FirstOrDefault(s => WarpEntrance.TerrainType.Groupings.Contains(s.TerrainTypeGrouping));

            int warpCost = WarpEntrance.TerrainType.WarpCost;
            if (warpCostSet is not null) warpCost = warpCostSet.Value;
            else if (warpCostMod is not null) warpCost += warpCostMod.Value;

            warpCost = Math.Max(0, warpCost); //enforce minimum

            return warpCost;
        }

    }
}
