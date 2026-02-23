using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map
{
    #region Interface

    /// <inheritdoc cref="TileObjectInstance"/>
    public interface ITileObjectInstance
    {
        /// <inheritdoc cref="TileObjectInstance.ID"/>
        int ID { get; }

        /// <inheritdoc cref="TileObjectInstance.TileObject"/>
        ITileObject TileObject { get; }

        /// <inheritdoc cref="TileObjectInstance.OriginTiles"/>
        ITile[] OriginTiles { get; }

        /// <inheritdoc cref="TileObjectInstance.AnchorCoordinate"/>
        ICoordinate AnchorCoordinate { get; }

        /// <inheritdoc cref="TileObjectInstance.HP"/>
        IHealthPoints HP { get; }

        /// <inheritdoc cref="TileObjectInstance.AttackRange"/>
        List<ICoordinate> AttackRange { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing an instance of a <c>ITileObject</c> located on a specific <c>ITile</c>.
    /// </summary>
    public class TileObjectInstance : ITileObjectInstance
    {
        #region Attributes

        /// <summary>
        /// Unique ID that identifies this tile object instance.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// The <c>ITileObject</c> located on the tile. 
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(MatchableNameConverter))]
        public ITileObject TileObject { get; private set; }

        /// <summary>
        /// List of <c>Tile</c>s from which the tile object's range originates.
        /// </summary>
        [JsonIgnore]
        public ITile[] OriginTiles { get; private set; }

        /// <summary>
        /// The anchor coordinate for the tile object. Assumes this is the always first tile in the OriginTiles list.
        /// </summary>
        public ICoordinate AnchorCoordinate { get { return this.OriginTiles.First()?.Coordinate; } }

        /// <summary>
        /// The tile object's current HP values.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IHealthPoints HP { get; private set; }

        /// <summary>
        /// List of the coordinates this tile object is capable of attacking.
        /// </summary>
        public List<ICoordinate> AttackRange { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tileObjectID">ID for identifying this particular tile object instance. Should be unique.</param>
        public TileObjectInstance(MapObjectsConfig config, int tileObjectID, IEnumerable<string> data, IMapObj map, IDictionary<string, ITileObject> tileObjects)
        {
            this.ID = tileObjectID;

            string name = DataParser.String(data, config.Name, "Name");
            string coordString = DataParser.String(data, config.Coordinate, "Coordinate");
            ICoordinate anchor = new Coordinate(map.Constants.CoordinateFormat, coordString);

            this.TileObject = System.TileObject.MatchName(tileObjects, name, anchor);

            if(config.HP is not null)
            {
                //Only try to parse the HP if we have at least one value set
                string currentHP = DataParser.OptionalString(data, config.HP.Current, "Current Durability");
                if(!string.IsNullOrEmpty(currentHP))
                    this.HP = new HealthPoints(data, config.HP);
            }

            this.OriginTiles = CalculateOriginTiles(map, anchor);
            this.AttackRange = new List<ICoordinate>();
        }

        /// <summary>
        /// Returns an array of the tile object instance's origin tiles, originating from <paramref name="anchorCoord"/>.
        /// </summary>
        private ITile[] CalculateOriginTiles(IMapObj map, ICoordinate anchorCoord)
        {
            int numberOfOriginTiles = (int)Math.Pow(this.TileObject.Size, 2);
            ITile[] originTiles = new ITile[numberOfOriginTiles];

            int index = 0;
            for (int r = 0; r < this.TileObject.Size; r++)
            {
                for(int c = 0; c < this.TileObject.Size; c++)
                {
                    //Calculate the x and y values of the tile
                    int x = anchorCoord.X + c;
                    int y = anchorCoord.Y + r;

                    ITile tile = map.GetTileByCoord(x, y);
                    originTiles[index++] = tile;

                    //Add this tile object inst to the tile objects list of each tile it occupies
                    tile.TileObjects.Add(this);
                }
            }

            return originTiles;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>TileObjectInstance</c> from each valid row.
        /// </summary>
        /// <remarks>The returned dictionary's key is a unique ID for each tile object instance.</remarks>
        /// <exception cref="TileObjectInstanceProcessingException"></exception>
        public static IDictionary<int, ITileObjectInstance> BuildDictionary(MapObjectsConfig config, IMapObj map, IDictionary<string, ITileObject> tileObjects)
        {
            IDictionary<int, ITileObjectInstance> tileObjectInsts = new Dictionary<int, ITileObjectInstance>();
            if (config?.Query is null) return tileObjectInsts;

            int idIterator = 1;
            foreach (IList<object> row in config.Query.Data)
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

                    tileObjectInsts.Add(idIterator, new TileObjectInstance(config, idIterator, tileObj, map, tileObjects));
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
