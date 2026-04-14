using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using System;

namespace RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses
{
    /// <summary>
    /// Container class for deserialized JSON <c>"WeaponRankBonus"</c> object data.
    /// </summary>
    public class WeaponRankBonusesConfig : Queryable
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of a weapon rank bonus category.
        /// </summary>
        [JsonRequired]
        public (int, int) Category { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. Location of a weapon rank bonus rank.
        /// </summary>
        public (int, int) Rank { get; set; } = (-1, -1);

        /// <summary>
        /// Optional. Collection of combat stat modifiers for a weapon rank bonus.
        /// </summary>
        public NamedStatConfig[] CombatStatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        /// <summary>
        /// Optional. Collection of stat modifiers for a weapon rank bonus.
        /// </summary>
        public NamedStatConfig[] StatModifiers { get; set; } = Array.Empty<NamedStatConfig>();

        #endregion Optional Fields
    }
}
