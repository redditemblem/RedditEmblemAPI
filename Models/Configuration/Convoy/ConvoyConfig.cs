using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.Convoy
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Convoy"</c> object data.
    /// </summary>
    public class ConvoyConfig : Queryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a convoy item's name value.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        /// <summary>
        /// Required. Location of a convoy item's owner value.
        /// </summary>
        [JsonRequired]
        public (int, int) Owner { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of a convoy item's uses.
        /// </summary>
        public (int, int) Uses { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a convoy item's value.
        /// </summary>
        public (int, int) Value { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of a convoy item's quantity value.
        /// </summary>
        public (int, int) Quantity { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations of a convoy item's engravings.
        /// </summary>
        public (int, int)[] Engravings { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
