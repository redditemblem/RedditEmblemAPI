using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.Battalions
{
    public class GambitRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for an gambit's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Minimum { get; set; }

        /// <summary>
        /// Required. Cell index for an gambit's maximum range value.
        /// </summary>
        [JsonRequired]
        public int Maximum { get; set; }

        #endregion
    }
}
