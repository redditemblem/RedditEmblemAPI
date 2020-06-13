using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Convoy
{
    public class ConvoyConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index of the convoy item's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index of the convoy item's owner.
        /// </summary>
        [JsonRequired]
        public int Owner { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Cell index of the convoy item's value.
        /// </summary>
        public int Value { get; set; } = -1;

        /// <summary>
        /// Cell index of the convoy item's quantity.
        /// </summary>
        public int Quantity { get; set; } = -1;

        #endregion
    }
}
