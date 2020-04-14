using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Affiliations
{
    public class AffiliationsConfig
    {
        #region RequiredFields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index of the affiliation's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index of the affiliation's grouping.
        /// </summary>
        [JsonRequired]
        public int Grouping { get; set; }

        #endregion

        #region OptionalFields

        /// <summary>
        /// List of cell indexes for the affiliation's text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
