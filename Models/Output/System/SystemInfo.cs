using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Services.Helpers;
using System;
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

            ParseRequiredData(config);
            ParseOptionalData(config);
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
            this.TerrainTypes = new Dictionary<string, TerrainType>();
            foreach (IList<object> row in config.TerrainTypes.Query.Data)
            {
                try
                {
                    IList<string> type = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(type, config.TerrainTypes.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!this.TerrainTypes.TryAdd(name, new TerrainType(config.TerrainTypes, type)))
                        throw new NonUniqueObjectNameException("terrain type");
                }
                catch (Exception ex)
                {
                    throw new TerrainTypeProcessingException((row.ElementAtOrDefault(config.TerrainTypes.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.Affiliations = new Dictionary<string, Affiliation>();
            foreach (IList<object> row in config.Affiliations.Query.Data)
            {
                try
                {
                    IList<string> aff = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(aff, config.Affiliations.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!this.Affiliations.TryAdd(name, new Affiliation(config.Affiliations, aff)))
                        throw new NonUniqueObjectNameException("affiliation");
                }
                catch (Exception ex)
                {
                    throw new AffiliationProcessingException((row.ElementAtOrDefault(config.Affiliations.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.Items = new Dictionary<string, Item>();
            foreach (IList<object> row in config.Items.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(item, config.Items.Name, "Name", false);
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!this.Items.TryAdd(name, new Item(config.Items, item)))
                        throw new NonUniqueObjectNameException("item");
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException((row.ElementAtOrDefault(config.Items.Name) ?? string.Empty).ToString(), ex);
                }
            }
        }

        /// <summary>
        /// Helper function for the constructor. Parses data into dictionaries.
        /// </summary>
        /// <param name="config"></param>
        private void ParseOptionalData(SystemConfig config)
        {
            this.TerrainEffects = new Dictionary<string, TerrainEffect>();
            if (config.TerrainEffects != null)
            {
                foreach (IList<object> row in config.TerrainEffects.Query.Data)
                {
                    try
                    {
                        IList<string> effect = row.Select(r => r.ToString()).ToList();
                        string name = ParseHelper.SafeStringParse(effect, config.TerrainEffects.Name, "Name", false);
                        if (string.IsNullOrEmpty(name)) continue;

                        if (!this.TerrainEffects.TryAdd(name, new TerrainEffect(config.TerrainEffects, effect)))
                            throw new NonUniqueObjectNameException("terrain effect");
                    }
                    catch (Exception ex)
                    {
                        throw new TerrainEffectProcessingException((row.ElementAtOrDefault(config.TerrainEffects.Name) ?? string.Empty).ToString(), ex);
                    }
                }
            }

            this.Classes = new Dictionary<string, Class>();
            if(config.Classes != null)
            {
                foreach (IList<object> row in config.Classes.Query.Data)
                {
                    try
                    {
                        IList<string> cls = row.Select(r => r.ToString()).ToList();
                        string name = ParseHelper.SafeStringParse(cls, config.Classes.Name, "Name", false);
                        if (string.IsNullOrEmpty(name)) continue;

                        if (!this.Classes.TryAdd(name, new Class(config.Classes, cls)))
                            throw new NonUniqueObjectNameException("class");
                    }
                    catch (Exception ex)
                    {
                        throw new ClassProcessingException((row.ElementAtOrDefault(config.Classes.Name) ?? string.Empty).ToString(), ex);
                    }
                }
            }
           
            this.Skills = new Dictionary<string, Skill>();
            if (config.Skills != null)
            {
                foreach (IList<object> row in config.Skills.Query.Data)
                {
                    try
                    {
                        IList<string> skill = row.Select(r => r.ToString()).ToList();
                        string name = ParseHelper.SafeStringParse(skill, config.Skills.Name, "Name", false);
                        if (string.IsNullOrEmpty(name)) continue;
                        
                        if (!this.Skills.TryAdd(name, new Skill(config.Skills, skill)))
                            throw new NonUniqueObjectNameException("skill");
                    }
                    catch (Exception ex)
                    {
                        throw new SkillProcessingException((row.ElementAtOrDefault(config.Skills.Name) ?? string.Empty).ToString(), ex);
                    }
                }
            }

            this.StatusConditions = new Dictionary<string, StatusCondition>();
            if (config.StatusConditions != null)
            {
                foreach (IList<object> row in config.StatusConditions.Query.Data)
                {
                    try
                    {
                        IList<string> stat = row.Select(r => r.ToString()).ToList();
                        string name = ParseHelper.SafeStringParse(stat, config.StatusConditions.Name, "Name", false);
                        if (string.IsNullOrEmpty(name)) continue;

                        if (!this.StatusConditions.TryAdd(name, new StatusCondition(config.StatusConditions, stat)))
                            throw new NonUniqueObjectNameException("status condition");
                    }
                    catch (Exception ex)
                    {
                        throw new StatusConditionProcessingException((row.ElementAtOrDefault(config.StatusConditions.Name) ?? string.Empty).ToString(), ex);
                    }
                }
            }

            this.Tags = new Dictionary<string, Tag>();
            if(config.Tags != null)
            {
                foreach (IList<object> row in config.Tags.Query.Data)
                {
                    try
                    {
                        IList<string> tag = row.Select(r => r.ToString()).ToList();
                        string name = ParseHelper.SafeStringParse(tag, config.Tags.Name, "Name", false);
                        if (string.IsNullOrEmpty(name)) continue;

                        if (!this.Tags.TryAdd(name, new Tag(config.Tags, tag)))
                            throw new NonUniqueObjectNameException("tag");
                    }
                    catch (Exception ex)
                    {
                        throw new TagProcessingException((row.ElementAtOrDefault(config.Tags.Name) ?? string.Empty).ToString(), ex);
                    }
                }
            }
        }

        #endregion
    }
}
