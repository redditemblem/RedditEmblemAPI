using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class RangeCalculationException : Exception
    {
        /// <summary>
        /// Container exception thrown when an error occurs during the calculation of a unit's range(s).
        /// </summary>
        public RangeCalculationException(IUnit unit, Exception innerException)
            : base($"An error occurred while calculating map ranges for unit \"{unit.Name}\".", innerException)
        { }

        /// <summary>
        /// Container exception thrown when an error occurs during the calculation of a tile objects's range(s).
        /// </summary>
        public RangeCalculationException(ITileObjectInstance tileObjectInst, Exception innerException)
            : base($"An error occurred while calculating map ranges for tile object \"{tileObjectInst.TileObject.Name}\" at coordinate \"{tileObjectInst.AnchorCoordinate.AsText}\".", innerException)
        { }
    }
}
