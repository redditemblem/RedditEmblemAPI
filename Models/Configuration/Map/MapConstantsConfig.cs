using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Map
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Constants"</c> object data.
    /// </summary>
    public class MapConstantsConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Height/width of each map tile measured in pixels.
        /// </summary>
        [JsonRequired]
        public int TileSize { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Flag signifying the existence of a header on the map image. Defaults to false.
        /// </summary>
        public bool HasHeaderTopLeft { get; set; } = false;

        /// <summary>
        /// Optional. Flag signifying the existence of a footer on the map image. Defaults to false.
        /// </summary>
        public bool HasHeaderBottomRight { get; set; } = false;

        /// <summary>
        /// Optional. The format for the map tile coordinates. Valid values are:
        /// <list type="bullet">
        /// <item>
        /// <term>0</term>
        /// <description>Default option. x,y format. (ex. "1,1")</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Alphanumerical format. (ex. "A1")</description>
        /// </item>
        /// </list>
        /// </summary>
        public CoordinateFormat CoordinateFormat { get; set; } = CoordinateFormat.XY;

        /// <summary>
        /// Optional. Flag indicating whether any ranges (unit or tile object) should be calculated at all. Defaults to true.
        /// </summary>
        public bool CalculateRanges { get; set; } = true;

        /// <summary>
        /// Optional. Flag signifying whether units in the back of pair-ups should have their ranges calculated. Defaults to true.
        /// </summary>
        public bool CalculatePairedUnitRanges { get; set; } = true;

        /// <summary>
        /// Optional. The maximum item range value that will be allowed to be calculated. Defaults to 15.
        /// </summary>
        public int ItemMaxRangeAllowedForCalculation { get; set; } = 15;

        /// <summary>
        /// Optional. The name of the Unit Stat to use as the movement stat when calculating ranges. Defaults to "Mov".
        /// </summary>
        public string UnitMovementStatName { get; set; } = "Mov";

        #endregion Optional Fields
    }

    public enum CoordinateFormat
    {
        XY = 0,
        Alphanumerical = 1
    }
}
