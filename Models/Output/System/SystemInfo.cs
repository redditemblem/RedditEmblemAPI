using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class SystemInfo
    {
        #region Constants

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        /// <summary>
        /// List of weapon ranks.
        /// </summary>
        [JsonIgnore]
        public IList<string> WeaponRanks { get; set; }

        #endregion

        #region Required Data

        /// <summary>
        /// Container dictionary for data about terrain types.
        /// </summary>
        public IDictionary<string, TerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        public IDictionary<string, Class> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about affiliations.
        /// </summary>
        public IDictionary<string, Affiliation> Affiliations { get; set; }

        /// <summary>
        /// Container dictionary for data about items.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        #endregion

        #region Optional Data

        /// <summary>
        /// Container dictionary for data about terrain effects.
        /// </summary>
        public IDictionary<string, TerrainEffect> TerrainEffects { get; set; }

        /// <summary>
        /// Container dictionary for data about skills.
        /// </summary>
        public IDictionary<string, Skill> Skills { get; set; }

        /// <summary>
        /// Container dictionary for data about status conditions.
        /// </summary>
        public IDictionary<string, StatusCondition> StatusConditions { get; set; }

        /// <summary>
        /// Container dictionary for data about tags.
        /// </summary>
        public IDictionary<string, Tag> Tags { get; set; }

        /// <summary>
        /// Container list for data about weapon rank bonuses.
        /// </summary>
        [JsonIgnore]
        public IList<WeaponRankBonus> WeaponRankBonuses { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        public SystemInfo(SystemConfig config)
        {
            //Copy over constants from config
            this.Currency = config.Currency;
            this.WeaponRanks = config.WeaponRanks;

            ParseOptionalData(config);
            ParseRequiredData(config); //some required data reliant on optional data
        }

        public void RemoveUnusedObjects()
        {
            //REQUIRED

            //Cull unused terrain types
            foreach (string key in this.TerrainTypes.Keys.ToList())
                if (!this.TerrainTypes[key].Matched)
                    this.TerrainTypes.Remove(key);

            //Cull unused affiliations
            foreach (string key in this.Affiliations.Keys.ToList())
                if (!this.Affiliations[key].Matched)
                    this.Affiliations.Remove(key);

            //Cull unused items
            foreach (string key in this.Items.Keys.ToList())
                if (!this.Items[key].Matched)
                    this.Items.Remove(key);

            //OPTIONAL

            //Cull unused terrain effects
            foreach (string key in this.TerrainEffects.Keys.ToList())
                if (!this.TerrainEffects[key].Matched)
                    this.TerrainEffects.Remove(key);

            //Cull unused classes
            foreach (string key in this.Classes.Keys.ToList())
                if (!this.Classes[key].Matched)
                    this.Classes.Remove(key);

            //Cull unused skills
            foreach (string key in this.Skills.Keys.ToList())
                if (!this.Skills[key].Matched)
                    this.Skills.Remove(key);

            //Cull unused status conditions
            foreach (string key in this.StatusConditions.Keys.ToList())
                if (!this.StatusConditions[key].Matched)
                    this.StatusConditions.Remove(key);

            //Cull unused tags
            foreach (string key in this.Tags.Keys.ToList())
                if (!this.Tags[key].Matched)
                    this.Tags.Remove(key);
        }

        #region Parsers

        /// <summary>
        /// Helper function for the constructor. Parses data into dictionaries.
        /// </summary>
        /// <param name="config"></param>
        private void ParseRequiredData(SystemConfig config)
        {
            this.TerrainTypes = TerrainType.BuildDictionary(config.TerrainTypes);
            this.Affiliations = Affiliation.BuildDictionary(config.Affiliations);
            this.Items = Item.BuildDictionary(config.Items, this.Tags); //note: items are dependent on Tags
            
        }

        /// <summary>
        /// Helper function for the constructor. Parses data into dictionaries.
        /// </summary>
        /// <param name="config"></param>
        private void ParseOptionalData(SystemConfig config)
        {
            if (config.TerrainEffects != null) this.TerrainEffects = TerrainEffect.BuildDictionary(config.TerrainEffects);
            else this.TerrainEffects = new Dictionary<string, TerrainEffect>();

            if (config.Classes != null) this.Classes = Class.BuildDictionary(config.Classes);
            else this.Classes = new Dictionary<string, Class>();

            if (config.Skills != null) this.Skills = Skill.BuildDictionary(config.Skills);
            else this.Skills = new Dictionary<string, Skill>();

            if (config.StatusConditions != null) this.StatusConditions = StatusCondition.BuildDictionary(config.StatusConditions);
            else this.StatusConditions = new Dictionary<string, StatusCondition>();

            if (config.Tags != null) this.Tags = Tag.BuildDictionary(config.Tags);
            else this.Tags = new Dictionary<string, Tag>();

            if (config.WeaponRankBonuses != null) this.WeaponRankBonuses = WeaponRankBonus.BuildList(config.WeaponRankBonuses);
            else this.WeaponRankBonuses = new List<WeaponRankBonus>();
        }

        #endregion
    }
}
