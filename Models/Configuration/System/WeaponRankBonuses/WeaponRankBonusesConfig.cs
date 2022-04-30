using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses
{
    /// <summary>
    /// Container class for deserialized JSON <c>"WeaponRankBonus"</c> object data.
    /// </summary>
    public class WeaponRankBonusesConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        /// <summary>
        /// Required. Cell index of a weapon rank bonus category.
        /// </summary>
        [JsonRequired]
        public int Category { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Cell index of a weapon rank bonus rank.
        /// </summary>
        public int Rank { get; set; } = -1;

        /// <summary>
        /// Optional. List of combat stat modifiers for a weapon rank bonus.
        /// </summary>
        public List<NamedStatConfig> CombatStatModifiers { get; set; } = new List<NamedStatConfig>();

        /// <summary>
        /// Optional. List of stat modifiers for a weapon rank bonus.
        /// </summary>
        public List<NamedStatConfig> StatModifiers { get; set; } = new List<NamedStatConfig>();

        #endregion
    }
}
