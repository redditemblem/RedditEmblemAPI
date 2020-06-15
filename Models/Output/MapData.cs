using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Wrapper class for the serialized JSON object data.
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// Container object for data about the map.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Container object for data about the system.
        /// </summary>
        public SystemData System { get; set; }

        /// <summary>
        /// Container list for data about units.
        /// </summary>
        public IList<Unit> Units { get; set; }

        /// <summary>
        /// Flag to show the convoy button in the toolbar.
        /// </summary>
        public bool ShowConvoyLink { get; set; }

        /// <summary>
        /// Flag to show the shop button in the toolbar.
        /// </summary>
        public bool ShowShopLink { get; set; }

        /// <summary>
        /// The URL of the chapter post on Reddit.
        /// </summary>
        public string ChapterPostURL { get; set; }

        public MapData(JSONConfiguration config)
        {
            this.ShowConvoyLink = (config.Convoy != null);
            this.ShowShopLink = (config.Shop != null);
            this.ChapterPostURL = string.Empty; //(values.ElementAtOrDefault(config.Team.Map.ChapterPostURL) ?? string.Empty).ToString();

            //Process data
            this.System = new SystemData(config.System);
            this.Map = new Map(config.Team.Map, this.System.TerrainTypes, this.System.TerrainEffects);
            this.Units = UnitsHelper.Process(config.Units, this.System, this.Map.Tiles);

            //Calculate unit ranges
            RangeHelper rangeHelper = new RangeHelper(this.Units, this.Map.Tiles);
            rangeHelper.CalculateUnitRange();

            //Clean up
            this.System.RemoveUnusedObjects();
        }

    }
}
