using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"WeaponRanks"</c> object data.
    /// </summary>
    public class UnitWeaponRanksConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Cell index for the weapon rank type.
        /// </summary>
        [JsonRequired]
        public int Type { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index for the weapon rank letter.
        /// </summary>
        public int Rank { get; set; } = -1;

        #endregion
    }
}
