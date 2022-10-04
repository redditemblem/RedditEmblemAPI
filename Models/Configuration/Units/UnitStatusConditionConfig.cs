using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitStatusConditionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for the name of the status condition.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for the number of remaining turns the status condition has.
        /// </summary>
        public int RemainingTurns { get; set; } = -1;

        /// <summary>
        /// Optional. List of configs for any additional stats the status condition may track.
        /// </summary>
        public List<NamedStatConfig> AdditionalStats { get; set; } = new List<NamedStatConfig>();

        #endregion
    }
}
