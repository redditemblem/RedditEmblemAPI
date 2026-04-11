using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.System.CombatArts
{
    public class CombatArtRangeConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a combat art's minimum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Minimum { get; set; }

        /// <summary>
        /// Required. Location of a combat art's maximum range value.
        /// </summary>
        [JsonRequired]
        public (int, int) Maximum { get; set; }

        #endregion Required Fields
    }
}
