using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class WeaponRankConfig
    {
        /// <summary>
        /// Cell index for the weapon rank type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        /// <summary>
        /// Cell index for the weapon rank letter.
        /// </summary>
        [JsonRequired]
        public int Rank { get; set; }
    }
}
