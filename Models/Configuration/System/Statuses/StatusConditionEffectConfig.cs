using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.Statuses
{
    public class StatusConditionEffectConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index of a status condition effect's type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        /// <summary>
        /// Required. List of cell indexes for the parameters.
        /// </summary>
        [JsonRequired]
        public IList<int> Parameters { get; set; }

        #endregion
    }
}
