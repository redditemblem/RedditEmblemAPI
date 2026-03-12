using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Helpers.Ranges.Items;
using RedditEmblemAPI.Helpers.Ranges.Movement;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
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
        public IMapObj Map { get; set; }

        /// <summary>
        /// Container object for data about the system.
        /// </summary>
        public SystemInfo System { get; set; }

        /// <summary>
        /// Container list for data about units.
        /// </summary>
        public List<IUnit> Units { get; set; }

        /// <summary>
        /// Workbook ID number from the Google Sheets URL.
        /// </summary>
        public string WorkbookID { get; set; }

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
            this.WorkbookID = (config.Team.AlternativeWorkbookID.Length > 0 ? config.Team.AlternativeWorkbookID : config.Team.WorkbookID);
            this.ShowConvoyLink = (config.Convoy != null);
            this.ShowShopLink = (config.Shop != null);

            //Process data, order is important on these
            this.System = new SystemInfo(config.System, config.Units.MovementType > -1);
            this.Map = new MapObj(config.Map, this.System.TerrainTypes, this.System.TileObjects);

            this.Units = UnitsHelper.Process(config.Units, this.System, this.Map);

            //Calculate map ranges
            if (config.Map.Constants.CalculateRanges)
            {
                MovementRangeCalculator movementCalc = new MovementRangeCalculator(this.Map, this.Units);
                movementCalc.CalculateUnitMovementRanges();

                ItemRangeCalculator itemCalc = new ItemRangeCalculator(this.Map, this.Units);
                itemCalc.CalculateTileObjectRanges();
                itemCalc.CalculateUnitItemRanges();
            }

            //Clean up
            this.System.RemoveUnusedObjects();
        }
    }
}
