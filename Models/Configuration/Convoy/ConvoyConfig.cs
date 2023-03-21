using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Convoy
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Convoy"</c> object data.
    /// </summary>
    public class ConvoyConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a convoy item's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of a convoy item's owner value.
        /// </summary>
        [JsonRequired]
        public int Owner { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a convoy item's uses.
        /// </summary>
        public int Uses { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a convoy item's value.
        /// </summary>
        public int Value { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index of a convoy item's quantity value.
        /// </summary>
        public int Quantity { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes of a convoy item's engravings.
        /// </summary>
        public List<int> Engravings { get; set; } = new List<int>();

        #endregion
    }
}
