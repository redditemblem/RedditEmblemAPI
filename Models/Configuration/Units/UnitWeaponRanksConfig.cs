using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitWeaponRanksConfig
    {
        #region Required Fields

        /// <summary>
        /// Cell index for the weapon rank type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Cell index for the weapon rank letter.
        /// </summary>
        public int Rank { get; set; } = -1;

        #endregion
    }
}
