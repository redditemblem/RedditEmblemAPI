using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Configuration.System.TerrainEffects;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"System"</c> object data.
    /// </summary>
    public class SystemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Container object for a system's affiliation configuration.
        /// </summary>
        [JsonRequired]
        public AffiliationsConfig Affiliations { get; set; }

        /// <summary>
        /// Required. Container object for a system's item configuration.
        /// </summary>
        [JsonRequired]
        public ItemsConfig Items { get; set; }

        /// <summary>
        /// Required. Container object for a system's terrain type configuration.
        /// </summary>
        [JsonRequired]
        public TerrainTypesConfig TerrainTypes { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Container object for a system's currency configuration.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; } = null;

        /// <summary>
        /// Optional. List of the weapon rank letters for this system, in order from lowest to highest. (ex. "E","D","C"...)
        /// </summary>
        public List<string> WeaponRanks { get; set; } = new List<string>();

        /// <summary>
        /// Optional. Container object for a system's class configuration.
        /// </summary>
        public ClassesConfig Classes { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's skill configuration.
        /// </summary>
        public SkillsConfig Skills { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's status condition configuration.
        /// </summary>
        public StatusConditionConfig StatusConditions { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's tag configuration.
        /// </summary>
        public TagConfig Tags { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's weapon rank bonus configuration.
        /// </summary>
        public WeaponRankBonusesConfig WeaponRankBonuses { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's terrain effect configuration.
        /// </summary>
        public TerrainEffectsConfig TerrainEffects { get; set; } = null;

        #endregion
    }
}
