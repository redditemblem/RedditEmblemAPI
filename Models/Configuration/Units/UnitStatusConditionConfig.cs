using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitStatusConditionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of the name of the status condition.
        /// </summary>
        [JsonRequired]
        public (int, int) Name { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of the number of remaining turns the status condition has.
        /// </summary>
        public (int, int) RemainingTurns { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of configs for any additional stats the status condition may track.
        /// </summary>
        public NamedStatConfig[] AdditionalStats { get; set; } = Array.Empty<NamedStatConfig>();

        #endregion Optional Fields
    }
}
