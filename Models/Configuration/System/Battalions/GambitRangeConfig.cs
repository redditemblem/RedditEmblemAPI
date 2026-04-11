using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    public class GambitRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of an gambit's minimum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Minimum { get; set; }

        /// <summary>
        /// Required. Location of an gambit's maximum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Maximum { get; set; }

        #endregion Required Fields
    }
}
