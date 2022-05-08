using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    /// <summary>
    /// Container class for deserialized JSON <c>"WeaponRanks"</c> object data.
    /// </summary>
    public class UnitWeaponRanksConfig
    {
        #region Optional Fields

        /// <summary>
        /// Optional. If weapon ranks are in fixed cells, the name of the weapon rank type. Mutually exclusive with <c>Type</c>.
        /// </summary>
        public string SourceName { get; set; } = string.Empty;

        /// <summary>
        /// Optional. If weapon ranks are dynamic, the cell index for the weapon rank type. Mutually exclusive with <c>SourceName</c>.
        /// </summary>
        public int Type { get; set; } = -1;

        /// <summary>
        /// Optional. Cell index for the weapon rank letter.
        /// </summary>
        public int Rank { get; set; } = -1;

        #endregion
    }
}
