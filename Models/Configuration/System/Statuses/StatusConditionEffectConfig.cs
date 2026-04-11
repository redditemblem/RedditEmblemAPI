using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Statuses
{
    public class StatusConditionEffectConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a status condition effect's type.
        /// </summary>
        [JsonRequired]
        public (int, int) Type { get; set; }

        /// <summary>
        /// Required. Collection of locations of a status condition's parameters.
        /// </summary>
        [JsonRequired]
        public (int, int)[] Parameters { get; set; }

        #endregion Required Fields
    }
}
