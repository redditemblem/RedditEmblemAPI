﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Models.Configuration.System.Items;
using RedditEmblemAPI.Models.Configuration.System.Skills;
using RedditEmblemAPI.Models.Configuration.System.TerrainTypes;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.System
{
    /// <summary>
    /// Container class for deserialized JSON <c>"System"</c> object data.
    /// </summary>
    public class SystemConfig
    {
        #region RequiredValues

        [JsonRequired]
        public IList<string> WeaponRanks { get; set; }

        /// <summary>
        /// Container object for a system's item configuration.
        /// </summary>
        [JsonRequired]
        public ItemsConfig Items { get; set; }

        /// <summary>
        /// Container object for a system's skill configuration.
        /// </summary>
        [JsonRequired]
        public SkillsConfig Skills { get; set; }

        /// <summary>
        /// Container object for a system's class configuration.
        /// </summary>
        [JsonRequired]
        public ClassesConfig Classes { get; set; }

        /// <summary>
        /// Container object for a system's affiliation configuration.
        /// </summary>
        [JsonRequired]
        public AffiliationsConfig Affiliations { get; set; }

        /// <summary>
        /// Container object for a system's terrain type configuration.
        /// </summary>
        [JsonRequired]
        public TerrainTypesConfig TerrainTypes { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Container object for a system's currency configuration.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        #endregion
    }
}
