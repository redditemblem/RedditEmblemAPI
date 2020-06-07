using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Statuses
{
    public class StatusConditionConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Cell index for the status name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        /// <summary>
        /// Cell index for the status type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Cell index for turns.
        /// </summary>
        public int Turns { get; set; } = -1;

        /// <summary>
        /// List of cell indexes for text fields.
        /// </summary>
        public IList<int> TextFields { get; set; } = new List<int>();

        #endregion
    }
}
