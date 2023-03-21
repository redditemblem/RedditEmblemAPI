using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Affiliations
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Affiliations"</c> object data.
    /// </summary>
    public class AffiliationsConfig : IQueryable
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of an affiliation's name value.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Required. Cell index of an affiliation's grouping value.
        /// </summary>
        [JsonRequired]
        public int Grouping { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of an affiliation's flip unit sprites flag.
        /// </summary>
        public int FlipUnitSprites { get; set; } = -1;

        /// <summary>
        /// Optional. List of cell indexes for an affiliation's text fields.
        /// </summary>
        public List<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
