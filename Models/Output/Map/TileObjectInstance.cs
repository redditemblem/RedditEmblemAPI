using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Object representing a <c>TileObject</c> instance located on a <c>Tile</c>.
    /// </summary>
    public class TileObjectInstance
    {
        /// <summary>
        /// Unique ID that identifies this tile object instance.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Only for JSON serialization. The name of the tile object.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.TileObject.Name; } }

        /// <summary>
        /// The <c>ITileObject</c> located on the tile. 
        /// </summary>
        [JsonIgnore]
        public ITileObject TileObject { get; set; }

        /// <summary>
        /// List of <c>Tile</c>s from which the tile object's range originates.
        /// </summary>
        [JsonIgnore]
        public List<Tile> OriginTiles { get; set; }

        /// <summary>
        /// The anchor coordinate for the tile object.
        /// </summary>
        [JsonIgnore]
        public Coordinate AnchorCoordinateObj { get; private set; }

        /// <summary>
        /// Coordinate of the anchor tile. Assumes this is the always first tile in the OriginTiles list.
        /// </summary>
        [JsonProperty]
        private string AnchorCoordinate { get { return this.OriginTiles.First()?.Coordinate.AsText; } }

        /// <summary>
        /// The tile object's current HP values.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HP HP { get; private set; }

        /// <summary>
        /// List of the coordinates this tile object is capable of attacking.
        /// </summary>
        public List<Coordinate> AttackRange { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        [Obsolete("Leaving this constructor in until all teams using the old Tile Object placement method are finished.")]
        public TileObjectInstance(int tileObjectID, ITileObject tileObject)
        {
            this.ID = tileObjectID;
            this.TileObject = tileObject;
            this.OriginTiles = new List<Tile>();
            this.AttackRange = new List<Coordinate>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tileObjectID">ID for identifying this particular tile object instance. Should be unique.</param>
        public TileObjectInstance(MapObjectsConfig config, MapConstantsConfig mapConstants, int tileObjectID, IEnumerable<string> data, IDictionary<string, ITileObject> tileObjects)
        {
            this.ID = tileObjectID;
            this.OriginTiles = new List<Tile>();
            this.AttackRange = new List<Coordinate>();

            string name = DataParser.String(data, config.Name, "Name");
            string coordString = DataParser.String(data, config.Coordinate, "Coordinate");

            this.AnchorCoordinateObj = new Coordinate(mapConstants.CoordinateFormat, coordString);
            this.TileObject = System.TileObject.MatchName(tileObjects, name, this.AnchorCoordinateObj);

            if(config.HP != null)
            {
                //Only try to parse the HP if we have at least one value set
                string currentHP = DataParser.OptionalString(data, config.HP.Current, "Current Durability");
                if(!string.IsNullOrEmpty(currentHP))
                    this.HP = new HP(data, config.HP);
            }
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>TileObjectInstance</c> from each valid row.
        /// </summary>
        /// <remarks>The returned dictionary's key is a unique ID for each tile object instance.</remarks>
        /// <exception cref="TileObjectInstanceProcessingException"></exception>
        public static Dictionary<int, TileObjectInstance> BuildDictionary(MapObjectsConfig config, MapConstantsConfig mapConstantsConfig, IDictionary<string, ITileObject> tileObjects)
        {
            Dictionary<int, TileObjectInstance> tileObjectInsts = new Dictionary<int, TileObjectInstance>();
            if (config == null || config.Query == null)
                return tileObjectInsts;

            int idIterator = 1;
            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;

                try
                {
                    IEnumerable<string> tileObj = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(tileObj, config.Name, "Name");
                    string coordinate = DataParser.OptionalString(tileObj, config.Coordinate, "Coordinate");

                    //Don't bother to parse this row if it hasn't been placed on the map
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(coordinate))
                        continue;

                    tileObjectInsts.Add(idIterator, new TileObjectInstance(config, mapConstantsConfig, idIterator, tileObj, tileObjects));
                    idIterator++;
                }
                catch (Exception ex)
                {
                    throw new TileObjectInstanceProcessingException(name, ex);
                }
            }

            return tileObjectInsts;
        }

        #endregion Static Functions
    }
}
