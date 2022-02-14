using RedditEmblemAPI.Models.Output.Map;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Container class for storing data about a unit's ranges.
    /// </summary>
    public class UnitRangeData
    {
        #region Attributes

        /// <summary>
        /// List of tiles that the unit is capable of moving to.
        /// </summary>
        public IList<Coordinate> Movement { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of attacking.
        /// </summary>
        public IList<Coordinate> Attack { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of using a utility item on.
        /// </summary>
        public IList<Coordinate> Utility { get; set; }

        #endregion Attributes

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UnitRangeData()
        {
            this.Movement = new List<Coordinate>();
            this.Attack = new List<Coordinate>();
            this.Utility = new List<Coordinate>();
        }
    }
}
