using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Configuration.System.WeaponRankBonuses;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"System"</c> object data.
    /// </summary>
    public class SystemConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Container object for constant system values.
        /// </summary>
        [JsonRequired]
        public SystemConstantsConfig Constants { get; set; }

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
        public TagsConfig Tags { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's weapon rank bonus configuration.
        /// </summary>
        public WeaponRankBonusesConfig WeaponRankBonuses { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's engravings configuration.
        /// </summary>
        public EngravingsConfig Engravings { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's tile objects configuration.
        /// </summary>
        public TileObjectsConfig TileObjects { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's battalion configuration.
        /// </summary>
        public BattalionsConfig Battalions { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's gambit configuration.
        /// </summary>
        public GambitsConfig Gambits { get; set; } = null;

        /// <summary>
        /// Optional. Container object for a system's emblem configuration.
        /// </summary>
        public EmblemsConfig Emblems { get; set; } = null;

        #endregion
    }
}
