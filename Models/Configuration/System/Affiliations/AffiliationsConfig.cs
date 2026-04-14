using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.Affiliations
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Affiliations"</c> object data.
    /// </summary>
    public class AffiliationsConfig : MultiQueryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of an affiliation's grouping value.
        /// </summary>
        [JsonRequired]
        public (int, int) Grouping { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of an affiliation's sprite URL.
        /// </summary>
        public (int, int) SpriteURL { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Location of an affiliation's flip unit sprites flag.
        /// </summary>
        public (int, int) FlipUnitSprites { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of locations for an affiliation's text fields.
        /// </summary>
        public (int, int)[] TextFields { get; set; } = Array.Empty<(int, int)>();

        #endregion Optional Fields
    }
}
