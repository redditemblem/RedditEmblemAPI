using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
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
        IList<ICoordinate> AttackRange { get; set; }
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
        public IList<ICoordinate> AttackRange { get; set; }

        #endregion Attributes

        #region Constructor

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

            if (config.HP is not null)
            {
                //Only try to parse the HP if we have at least one value set
                string currentHP = DataParser.OptionalString(data, config.HP.Current, "Current Durability");
                if (!string.IsNullOrEmpty(currentHP))
                    this.HP = new HealthPoints(data, config.HP);
            }

            this.OriginTiles = CalculateOriginTiles(map, anchor);
            this.AttackRange = new List<ICoordinate>();
        }

        /// <summary>
        /// Constructor. Copies values from <paramref name="copyFrom"/> into a new tile object instance located at <paramref name="anchor"/>.
        /// </summary>
        public TileObjectInstance(int tileObjectID, ICoordinate anchor, ITileObjectInstance copyFrom, IMapObj map)
        {
            this.ID = tileObjectID;

            this.TileObject = copyFrom.TileObject;
            this.HP = copyFrom.HP;

            this.OriginTiles = CalculateOriginTiles(map, anchor);
            this.AttackRange = new List<ICoordinate>();
        }

        #endregion Constructor

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
                for (int c = 0; c < this.TileObject.Size; c++)
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
                string coordinate = string.Empty;
                IEnumerable<string> tileObj;
                ITileObjectInstance tileObjectInst;

                try
                {
                    tileObj = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(tileObj, config.Name, "Name");
                    coordinate = DataParser.OptionalString(tileObj, config.Coordinate, "Coordinate");

                    //Don't bother to parse this row if it hasn't been placed on the map
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(coordinate))
                        continue;

                    tileObjectInst = new TileObjectInstance(config, idIterator, tileObj, map, tileObjects);
                    tileObjectInsts.Add(idIterator++, tileObjectInst);
                }
                catch (Exception ex)
                {
                    throw new TileObjectInstanceProcessingException(name, coordinate, ex);
                }

                //Check if the newly created tile object instance is setup to repeat itself
                if (config.RepeaterTool is not null)
                {
                    try
                    {
                        string shapeVal = DataParser.OptionalString(tileObj, config.RepeaterTool.Shape, "Repeater Tool Shape");
                        if (!string.IsNullOrEmpty(shapeVal))
                        {
                            TileObjectInstanceRepeaterShape shape = GetTileObjectInstanceRepeaterShape(shapeVal);
                            int height = DataParser.Int_NonZeroPositive(tileObj, config.RepeaterTool.Height, "Repeater Tool Height");
                            int width = DataParser.Int_NonZeroPositive(tileObj, config.RepeaterTool.Width, "Repeater Tool Width");

                            IEnumerable<ITileObjectInstance> copies = RepeatTileObjectInstance(map, tileObjectInst, ref idIterator, shape, height, width);
                            foreach (ITileObjectInstance copy in copies)
                                tileObjectInsts.Add(copy.ID, copy);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new TileObjectInstanceProcessingException(name, coordinate, ex);
                    }
                }
            }

            return tileObjectInsts;
        }

        /// <summary>
        /// Matches <paramref name="shape"/> to a TileObjectInstanceRepeaterShape value and returns it.
        /// </summary>
        /// <exception cref="UnmatchedTileObjectInstanceRepeaterShapeException"></exception>
        private static TileObjectInstanceRepeaterShape GetTileObjectInstanceRepeaterShape(string shape)
        {
            object shapeEnum;
            if (!Enum.TryParse(typeof(TileObjectInstanceRepeaterShape), shape, out shapeEnum))
                throw new UnmatchedTileObjectInstanceRepeaterShapeException(shape, Enum.GetNames<TileObjectInstanceRepeaterShape>());

            return (TileObjectInstanceRepeaterShape)shapeEnum;
        }

        private static IEnumerable<ITileObjectInstance> RepeatTileObjectInstance(IMapObj map, ITileObjectInstance copyFrom, ref int idIterator, TileObjectInstanceRepeaterShape shape, int height, int width)
        {
            switch (shape)
            {
                case TileObjectInstanceRepeaterShape.Diamond:
                    return RepeatTileObjectInstanceInDiamond(map, copyFrom, ref idIterator, height, width);

                case TileObjectInstanceRepeaterShape.Rectangle:
                    return RepeatTileObjectInstanceInRectangle(map, copyFrom, ref idIterator, height, width);

                default:
                    return new List<ITileObjectInstance>();
            }
        }

        #region Diamond Shaped Repeater

        private static IEnumerable<ITileObjectInstance> RepeatTileObjectInstanceInDiamond(IMapObj map, ITileObjectInstance copyFrom, ref int idIterator, int height, int width)
        {
            if (height % 2 == 0 || width % 2 == 0)
                throw new ArgumentException("A \"Diamond\" shaped repeater must have an odd-numbered height and width.");
            if (height < 3 || width < 3)
                throw new ArgumentException("A \"Diamond\" shaped repeater must have a height and width of at least 3 tiles.");

            List<ITileObjectInstance> copies = new List<ITileObjectInstance>();

            ICoordinate copyFromAnchor = copyFrom.AnchorCoordinate;
            IMapSegment segment = map.GetSegmentByCoord(copyFromAnchor);
            int size = copyFrom.TileObject.Size;

            if (height * size > segment.HeightInTiles)
                throw new ArgumentException("The calculated height (height * tile object size) of a \"Diamond\" shaped repeater cannot exceed the height of the map segment.");
            if (width * size > segment.WidthInTiles)
                throw new ArgumentException("The calculated width (width * tile object size) of a \"Diamond\" shaped repeater cannot exceed the width of the map segment.");

            int verticalRadius = (int)Math.Floor(height / 2m);
            int horizontalRadius = (int)Math.Floor(width / 2m);

            for (int x = 0; x <= horizontalRadius; x++)
            {
                for (int y = 0; y <= verticalRadius; y++)
                {
                    if (x == 0 && y == 0) continue; //ignore center
                    if (!IsPointInsideTriangle(horizontalRadius, verticalRadius, x, y)) break; //enforces a slope between the max horizontal and max vertical coords

                    int xMod = x * size;
                    int yMod = y * size;

                    //Attempt to grab the anchor tiles for this round of repeats
                    ITile[] anchors = new ITile[4];
                    try { anchors[0] = segment.GetTileByCoord(copyFromAnchor.X + xMod, copyFromAnchor.Y + yMod); } catch (TileOutOfBoundsException) { }
                    try { anchors[1] = segment.GetTileByCoord(copyFromAnchor.X - xMod, copyFromAnchor.Y + yMod); } catch (TileOutOfBoundsException) { }
                    try { anchors[2] = segment.GetTileByCoord(copyFromAnchor.X + xMod, copyFromAnchor.Y - yMod); } catch (TileOutOfBoundsException) { }
                    try { anchors[3] = segment.GetTileByCoord(copyFromAnchor.X - xMod, copyFromAnchor.Y - yMod); } catch (TileOutOfBoundsException) { }

                    //Attempt to generate tile object instances for each distinct anchor found
                    //Ignore any exceptions thrown
                    foreach(ITile anchor in anchors.Distinct())
                    {
                        if (anchor is null) continue;
                            
                        try { copies.Add(new TileObjectInstance(idIterator++, anchor.Coordinate, copyFrom, map)); } 
                        catch { }
                    }
                }
            }

            return copies;
        }

        /// <summary>
        /// Calculates and returns the area of the triange with points (x1, y1), (x2, y2), and (x3, y3).
        /// </summary>
        private static double GetAreaOfTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            return Math.Abs((x1 * (y2 - y3) +
                             x2 * (y3 - y1) +
                             x3 * (y1 - y2)) / 2.0);
        }

        /// <summary>
        /// Returns true if the point (x, y) lies inside of a right triangle located at (0, 0) with <paramref name="width"/> and <paramref name="height"/>.
        /// </summary>
        /// <see cref="https://www.geeksforgeeks.org/dsa/check-whether-a-given-point-lies-inside-a-triangle-or-not/"/>
        private static bool IsPointInsideTriangle(int width, int height, int x, int y)
        {
            /* Calculate area of triangle ABC */
            double A = GetAreaOfTriangle(width, 0, 0, height, 0, 0);

            /* Calculate area of triangle PBC */
            double A1 = GetAreaOfTriangle(x, y, 0, height, 0, 0);

            /* Calculate area of triangle PAC */
            double A2 = GetAreaOfTriangle(width, 0, x, y, 0, 0);

            /* Calculate area of triangle PAB */
            double A3 = GetAreaOfTriangle(width, 0, 0, height, x, y);

            /* Check if sum of A1, A2 and A3 is same as A */
            return (A == A1 + A2 + A3);
        }

        #endregion Diamond Shaped Repeater

        #region Rectangle Shaped Repeater

        private static IEnumerable<ITileObjectInstance> RepeatTileObjectInstanceInRectangle(IMapObj map, ITileObjectInstance copyFrom, ref int idIterator, int height, int width)
        {
            if (height < 2 || width < 2)
                throw new ArgumentException("A \"Rectangle\" shaped repeater must have a height and width of at least 2 tiles.");

            List<ITileObjectInstance> copies = new List<ITileObjectInstance>();

            ICoordinate copyFromAnchor = copyFrom.AnchorCoordinate;
            IMapSegment segment = map.GetSegmentByCoord(copyFromAnchor);
            int size = copyFrom.TileObject.Size;

            if (height * size > segment.HeightInTiles)
                throw new ArgumentException("The calculated height (height * tile object size) of a \"Rectangle\" shaped repeater cannot exceed the height of the map segment.");
            if (width * size > segment.WidthInTiles)
                throw new ArgumentException("The calculated width (width * tile object size) of a \"Rectangle\" shaped repeater cannot exceed the width of the map segment.");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 && y == 0) continue; //ignore anchor
                    
                    int xMod = x * size;
                    int yMod = y * size;

                    ITile tile = null;
                    try { tile = segment.GetTileByCoord(copyFromAnchor.X + xMod, copyFromAnchor.Y + yMod); } catch (TileOutOfBoundsException) { }

                    if (tile is null) continue;

                    try { copies.Add(new TileObjectInstance(idIterator++, tile.Coordinate, copyFrom, map)); }
                    catch { }
                }
            }

            return copies;
        }

        #endregion Rectangle Shaped Repeater

        #endregion Static Functions
    }

    public enum TileObjectInstanceRepeaterShape
    {
        Diamond,
        Rectangle
    }
}