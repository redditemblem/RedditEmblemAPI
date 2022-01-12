using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Wrapper class for the serialized JSON map object data.
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// Container object for data about the map.
        /// </summary>
        public MapObj Map { get; set; }

        /// <summary>
        /// Container object for data about the system.
        /// </summary>
        public SystemInfo System { get; set; }

        /// <summary>
        /// Container list for data about units.
        /// </summary>
        public IList<Unit> Units { get; set; }

        /// <summary>
        /// Flag indicating if the convoy button should be shown in the toolbar.
        /// </summary>
        public bool ShowConvoyLink { get; set; }

        /// <summary>
        /// Flag indicating if the shop button should be shown in the toolbar.
        /// </summary>
        public bool ShowShopLink { get; set; }

        public MapData(JSONConfiguration config)
        {
            this.ShowConvoyLink = (config.Convoy != null);
            this.ShowShopLink = (config.Shop != null);

            //Process data, order is important on these
            this.System = new SystemInfo(config.System);
            this.Map = new MapObj(config.Map, this.System.TerrainTypes, this.System.TerrainEffects);

            this.Units = UnitsHelper.Process(config.Units, this.System, this.Map);

            //Calculate unit ranges
            RangeHelper rangeHelper = new RangeHelper(this.Units, this.Map);
            rangeHelper.CalculateUnitRanges();

            //Clean up
            this.System.RemoveUnusedObjects();
        }

    }
}
