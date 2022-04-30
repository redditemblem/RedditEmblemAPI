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
        public List<Coordinate> Movement { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of attacking.
        /// </summary>
        public List<Coordinate> Attack { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of using a utility item on.
        /// </summary>
        public List<Coordinate> Utility { get; set; }

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
