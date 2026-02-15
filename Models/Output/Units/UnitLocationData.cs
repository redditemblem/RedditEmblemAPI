using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitLocationData"/>
    public interface IUnitLocationData
    {
        /// <inheritdoc cref="UnitLocationData.CoordinateString"/>
        string CoordinateString { get; }

        /// <inheritdoc cref="UnitLocationData.Coordinate"/>
        ICoordinate Coordinate { get; set; }

        /// <inheritdoc cref="UnitLocationData.UnitSize"/>
        int UnitSize { get; }

        /// <inheritdoc cref="UnitLocationData.AnchorTile"/>
        ITile AnchorTile { get; set; }

        /// <inheritdoc cref="UnitLocationData.OriginTiles"/>
        List<ITile> OriginTiles { get; set; }

        /// <inheritdoc cref="UnitLocationData.PairedUnitObj"/>
        IUnit PairedUnitObj { get; set; }

        /// <inheritdoc cref="UnitLocationData.IsBackOfPair"/>
        bool IsBackOfPair { get; set; }

        /// <inheritdoc cref="UnitLocationData.IsOnMap()"/>
        bool IsOnMap();
    }

    #endregion Interface

    /// <summary>
    /// Container object for storing data about a unit's physical map location.
    /// </summary>
    public class UnitLocationData : IUnitLocationData
    {
        #region Attributes

        /// <summary>
        /// Unparsed, raw coordinate string value.
        /// </summary>
        [JsonIgnore]
        public string CoordinateString { get; private set; }

        /// <summary>
        /// The unit's location on the map.
        /// </summary>
        public ICoordinate Coordinate { get; set; }

        /// <summary>
        /// The size of the unit in grid tiles. Defaults to 1.
        /// </summary>
        public int UnitSize { get; private set; }

        /// <summary>
        /// The <c>Tile</c> that this unit is drawn at.
        /// </summary>
        [JsonIgnore]
        public ITile AnchorTile { get; set; }

        /// <summary>
        /// List of <c>Tile</c>s that this unit's range originates from.
        /// </summary>
        [JsonIgnore]
        public List<ITile> OriginTiles { get; set; }

        /// <summary>
        /// The <c>Unit</c> paired with the unit, if any.
        /// </summary>
        [JsonIgnore]
        public IUnit PairedUnitObj { get; set; }

        /// <summary>
        /// Flag indicating if the unit is sitting in the back of a pair.
        /// </summary>
        public bool IsBackOfPair { get; set; }

        #region JSON Serialization Only

        //// <summary>
        /// Only for JSON serialization. Returns the name of the <c>PairedUnit</c>. If <c>PairedUnit</c> is null, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string PairedUnit { get { return (this.PairedUnitObj == null ? string.Empty : this.PairedUnitObj.Name); } }


        #endregion JSON Serialization Only

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitLocationData(UnitsConfig config, IEnumerable<string> data)
        {
            this.OriginTiles = new List<ITile>();

            this.CoordinateString = DataParser.OptionalString(data, config.Coordinate, "Coordinate");
            this.UnitSize = DataParser.OptionalInt_NonZeroPositive(data, config.UnitSize, "Unit Size");
        }

        /// <summary>
        /// Returns true if the unit has origin tiles set and is not in the back of a pair-up.
        /// </summary>
        /// <returns></returns>
        public bool IsOnMap()
        {
            return this.OriginTiles.Any() && !this.IsBackOfPair;
        }
    }
}
