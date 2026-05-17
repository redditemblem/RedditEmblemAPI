using Newtonsoft.Json;
using System;

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
        [Obsolete("CanOnlyUseBeforeMovement will be replaced by ReduceMovementByToUse")]
        public (int, int) CanOnlyUseBeforeMovement { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of the number of tiles by which to reduce the unit's max movement in order to use this item.
        /// </summary>
        public (int, int) ReduceMovementByToUse { get; set; } = (-1, -1);

        #endregion Optional Fields
    }
}