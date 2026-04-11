using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Items
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Range"</c> object data.
    /// </summary>
    public class ItemRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of an item's minimum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Minimum { get; set; }

        /// <summary>
        /// Required. Location of an item's maximum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Maximum { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of an item's range shape value.
        /// </summary>
        public (int, int) Shape { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the flag for if an item can only be used before movement.
        /// </summary>
        public (int, int) CanOnlyUseBeforeMovement { get; set; } = (-1, -1);

        #endregion Optional Fields
    }
}