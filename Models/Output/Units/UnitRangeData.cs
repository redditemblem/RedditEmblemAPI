using RedditEmblemAPI.Models.Output.Map;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitRangeData"/>
    public interface IUnitRangeData
    {
        /// <inheritdoc cref="UnitRangeData.Movement"/>
        List<ICoordinate> Movement { get; set; }

        /// <inheritdoc cref="UnitRangeData.Attack"/>
        List<ICoordinate> Attack { get; set; }

        /// <inheritdoc cref="UnitRangeData.Utility"/>
        List<ICoordinate> Utility { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Container class for storing data about a unit's ranges.
    /// </summary>
    public class UnitRangeData : IUnitRangeData
    {
        #region Attributes

        /// <summary>
        /// List of tiles that the unit is capable of moving to.
        /// </summary>
        public List<ICoordinate> Movement { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of attacking.
        /// </summary>
        public List<ICoordinate> Attack { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of using a utility item on.
        /// </summary>
        public List<ICoordinate> Utility { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitRangeData()
        {
            this.Movement = new List<ICoordinate>();
            this.Attack = new List<ICoordinate>();
            this.Utility = new List<ICoordinate>();
        }
    }
}
