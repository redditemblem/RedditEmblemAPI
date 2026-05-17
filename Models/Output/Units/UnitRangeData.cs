using Newtonsoft.Json;
using RedditEmblemAPI.Models.Output.Map;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitRangeData"/>
    public interface IUnitRangeData
    {
        /// <inheritdoc cref="UnitRangeData.MovementWithMinimumCost"/>
        IDictionary<ICoordinate, int> MovementWithMinimumCost { get; set; }

        /// <inheritdoc cref="UnitRangeData.Attack"/>
        IEnumerable<ICoordinate> Attack { get; set; }

        /// <inheritdoc cref="UnitRangeData.Utility"/>
        IEnumerable<ICoordinate> Utility { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Container class for storing data about a unit's ranges.
    /// </summary>
    public class UnitRangeData : IUnitRangeData
    {
        #region Attributes

        /// <summary>
        /// Dictionary of coordinates that the unit is capable of moving to. Value is the minimum path cost for the unit to reach that coordinate.
        /// </summary>
        [JsonIgnore]
        public IDictionary<ICoordinate, int> MovementWithMinimumCost { get; set; }

        /// <summary>
        /// For JSON serialization only. Returns <c>this.MovementWithMinimumCost</c>'s coordinate key set.
        /// </summary>
        [JsonProperty]
        private IEnumerable<ICoordinate> Movement => this.MovementWithMinimumCost.Select(c => c.Key);

        /// <summary>
        /// Collection of coordinates that the unit is capable of attacking.
        /// </summary>
        public IEnumerable<ICoordinate> Attack { get; set; }

        /// <summary>
        /// Collection of coordinates that the unit is capable of using a utility item on.
        /// </summary>
        public IEnumerable<ICoordinate> Utility { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitRangeData()
        {
            this.MovementWithMinimumCost = new Dictionary<ICoordinate, int>();
            this.Attack = new List<ICoordinate>();
            this.Utility = new List<ICoordinate>();
        }
    }
}
