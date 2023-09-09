using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.CombatArts
{
    public class CombatArtRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for a combat art's minimum range value.
        /// </summary>
        [JsonRequired]
        public int Minimum { get; set; }

        /// <summary>
        /// Required. Cell index for a combat art's maximum range value.
        /// </summary>
        [JsonRequired]
        public int Maximum { get; set; }

        #endregion
    }
}
