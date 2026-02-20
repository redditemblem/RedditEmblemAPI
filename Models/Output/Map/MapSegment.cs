using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Map
{
    #region Interface

    /// <inheritdoc cref="MapSegment"/>
    public interface IMapSegment
    {
        /// <inheritdoc cref="MapSegment.ImageURL"/>
        string ImageURL { get; }

        /// <inheritdoc cref="MapSegment.HeightInPixels"/>
        int HeightInPixels { get; }

        /// <inheritdoc cref="MapSegment.WidthInPixels"/>
        int WidthInPixels { get; }

        /// <inheritdoc cref="MapSegment.HeightInTiles"/>
        int HeightInTiles { get; }

        /// <inheritdoc cref="MapSegment.WidthInTiles"/>
        int WidthInTiles { get; }

        /// <inheritdoc cref="MapSegment.HorizontalTileRangeWithinMap"/>
        Range HorizontalTileRangeWithinMap { get; }

        /// <inheritdoc cref="MapSegment.Tiles"/>
        ITile[][] Tiles { get; }

        /// <inheritdoc cref="MapSegment.CoordinateFallsWithinRange(ICoordinate)"/>
        bool CoordinateFallsWithinRange(ICoordinate coord);

        /// <inheritdoc cref="MapSegment.GetTileByCoord(ICoordinate)"/>
        ITile GetTileByCoord(ICoordinate coord);

        /// <inheritdoc cref="MapSegment.GetTileByCoord(int, int)"/>
        ITile GetTileByCoord(int x, int y);

        /// <inheritdoc cref="MapSegment.GetTilesInRadius(ICoordinate, int)"/>
        IEnumerable<ITile> GetTilesInRadius(ICoordinate center, int radius);
    }

    #endregion Interface

    /// <summary>
    /// Object representing a specific portion of the full map.
    /// </summary>
    public class MapSegment : IMapSegment
    {
        #region Attributes
        
        /// <summary>
        /// The map segment's background image URL.
        /// </summary>
        public string ImageURL { get; private set; }

        /// <summary>
        /// The height of the segment image in pixels.
        /// </summary>
        public int HeightInPixels { get; private set; }

        /// <summary>
        /// The width of the segment image in pixels.
        /// </summary>
        public int WidthInPixels { get; private set; }

        /// <summary>
        /// The height of the segment in tiles.
        /// </summary>
        public int HeightInTiles { get; private set; }

        /// <summary>
        /// The width of the segment in tiles.
        /// </summary>
        public int WidthInTiles { get; private set; }

        /// <summary>
        /// The horizontal range of tiles that this segment occupies within the map.
        /// </summary>
        public Range HorizontalTileRangeWithinMap { get; private set; }

        /// <summary>
        /// This segment's set of tiles.
        /// </summary>
        public ITile[][] Tiles { get; private set; }

        /// <summary>
        /// The coordinate format from the map constants.
        /// </summary>
        /// <remarks>
        ///  Adding a reference to this here for the sake of exceptions within this class.
        /// </remarks>
        private CoordinateFormat CoordinateFormat { get; set; }

        #endregion Attributes

        #region Constructor

        public MapSegment(MapConstantsConfig config, IImageLoader loader, string imageUrl, int beginningOfHorizontalRange)
        {
            this.CoordinateFormat = config.CoordinateFormat;
            this.ImageURL = DataParser.String_URL(imageUrl, "Map Image URL"); //validate URL

            int heightInPixels, widthInPixels, heightInTiles, widthInTiles;
            GetImageDimensions(loader, this.ImageURL, out heightInPixels, out widthInPixels);
            CalculateTileDimensions(config, heightInPixels, widthInPixels, out heightInTiles, out widthInTiles);

            this.HeightInPixels = heightInPixels;
            this.WidthInPixels = widthInPixels;
            this.HeightInTiles = heightInTiles;
            this.WidthInTiles = widthInTiles;

            this.HorizontalTileRangeWithinMap = new Range(beginningOfHorizontalRange, beginningOfHorizontalRange + widthInTiles - 1);
            this.Tiles = new ITile[heightInTiles][];
        }

        /// <summary>
        /// Outputs the <paramref name="heightInPixels"/> and <paramref name="widthInPixels"/> of the image file at <paramref name="imageUrl"/>.
        /// </summary>
        /// <exception cref="MapImageLoadFailedException"></exception>
        private void GetImageDimensions(IImageLoader imageLoader, string imageUrl, out int heightInPixels, out int widthInPixels)
        {
            try
            {
                imageLoader.GetImageDimensionsByUrl(imageUrl, out heightInPixels, out widthInPixels);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("404"))
                    throw new MapImageLoadFailedException();
                throw new MapImageLoadFailedException(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Based on the image's <paramref name="heightInPixels"/> and <paramref name="widthInPixels"/>, calculates the segment's expected <paramref name="heightInTiles"/> and <paramref name="widthInTiles"/>.
        /// </summary>
        private void CalculateTileDimensions(MapConstantsConfig config, int heightInPixels, int widthInPixels, out int heightInTiles, out int widthInTiles)
        {
            heightInTiles = (int)Math.Floor((decimal)heightInPixels / config.TileSize);
            widthInTiles = (int)Math.Floor((decimal)widthInPixels / config.TileSize);

            if (config.HasHeaderTopLeft)
            {
                heightInTiles--;
                widthInTiles--;
            }

            if (config.HasHeaderBottomRight)
            {
                heightInTiles--;
                widthInTiles--;
            }
        }

        #endregion Constructor

        /// <summary>
        /// Returns true if <paramref name="coord"/> falls within the horizontal range of tiles represented by this map segment.
        /// </summary>
        public bool CoordinateFallsWithinRange(ICoordinate coord)
        {
            return coord.X >= this.HorizontalTileRangeWithinMap.Start.Value && coord.X <= this.HorizontalTileRangeWithinMap.End.Value;
        }

        /// <summary>
        /// Finds and returns the tile located at <paramref name="coord"/>.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public ITile GetTileByCoord(ICoordinate coord)
        {
            try { return this.Tiles[coord.Y - 1][coord.X - 1]; }
            catch { throw new TileOutOfBoundsException(coord); }
        }

        /// <summary>
        /// Finds and returns the tile located at {<paramref name="x"/>, <paramref name="y"/>}.
        /// </summary>
        /// <exception cref="TileOutOfBoundsException"></exception>
        public ITile GetTileByCoord(int x, int y)
        {
            try { return this.Tiles[y - 1][x - 1]; }
            catch { throw new TileOutOfBoundsException(new Coordinate(this.CoordinateFormat, x, y)); }
        }

        /// <summary>
        /// Returns a list of distinct tiles that are within <paramref name="radius"/> tiles of the <paramref name="center"/>.
        /// </summary>
        public IEnumerable<ITile> GetTilesInRadius(ICoordinate center, int radius)
        {
            List<ITile> tiles = new List<ITile>();

            for (int x = 0; x <= radius; x++)
            {
                for (int y = 0; y <= radius; y++)
                {
                    if (x == 0 && y == 0) continue; //ignore center
                    if (x + y > radius) continue; //if the total displacement is greater than the radius, stop

                    //If fetching any tile fails, we still want to continue execution
                    try { tiles.Add(GetTileByCoord(center.X + x, center.Y + y)); } catch (TileOutOfBoundsException) { }
                    try { tiles.Add(GetTileByCoord(center.X - x, center.Y + y)); } catch (TileOutOfBoundsException) { }
                    try { tiles.Add(GetTileByCoord(center.X + x, center.Y - y)); } catch (TileOutOfBoundsException) { }
                    try { tiles.Add(GetTileByCoord(center.X - x, center.Y - y)); } catch (TileOutOfBoundsException) { }
                }
            }

            //Return distinct list of tiles
            return tiles.Distinct();
        }

        #region Static Functions

        public static IMapSegment[] BuildArray(MapConstantsConfig config, IImageLoader imageLoader, IEnumerable<string> mapImageURLs)
        {
            IMapSegment[] segments = new IMapSegment[mapImageURLs.Count()];

            int index = 0;
            foreach (string url in mapImageURLs)
            {
                int beginningOfHorizontalRange = 1;
                if (index > 0) beginningOfHorizontalRange += segments.Take(index).Sum(s => s.WidthInTiles);

                segments[index++] = new MapSegment(config, imageLoader, url, beginningOfHorizontalRange);
            }

            return segments;
        }

        #endregion Static Functions
    }
}
