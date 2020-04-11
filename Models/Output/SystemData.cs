using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    public class SystemData
    {
        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        public IDictionary<string, Class> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about items.
        /// </summary>
        public IDictionary<string, Item> Items { get; set; }

        /// <summary>
        /// Container dictionary for data about skills.
        /// </summary>
        public IDictionary<string, Skill> Skills { get; set; }

        /// <summary>
        /// Container dictionary for data about terrain types.
        /// </summary>
        public IDictionary<string, TerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }

        public SystemData(SystemConfig config)
        {
            this.Classes = new Dictionary<string, Class>();
            foreach (IList<object> row in config.Classes.Query.Data)
            {
                try
                {
                    IList<string> cls = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(cls.ElementAtOrDefault<string>(config.Classes.Name)))
                        continue;
                    this.Classes.Add(cls.ElementAtOrDefault<string>(config.Classes.Name),
                                     new Class(config.Classes, cls));
                }
                catch (Exception ex)
                {
                    throw new ClassProcessingException((row.ElementAtOrDefault(config.Classes.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.Items = new Dictionary<string, Item>();
            foreach (IList<object> row in config.Items.Query.Data)
            {
                try
                {
                    IList<string> item = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(item.ElementAtOrDefault<string>(config.Items.Name)))
                        continue;
                    this.Items.Add(item.ElementAtOrDefault(config.Items.Name), new Item(config.Items, item));
                }
                catch (Exception ex)
                {
                    throw new ItemProcessingException((row.ElementAtOrDefault(config.Items.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.Skills = new Dictionary<string, Skill>();
            foreach (IList<object> row in config.Skills.Query.Data)
            {
                try
                {
                    IList<string> skill = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(skill.ElementAtOrDefault<string>(config.Skills.Name)))
                        continue;
                    this.Skills.Add(skill.ElementAtOrDefault<string>(config.Skills.Name), new Skill(config.Skills, skill));
                }
                catch (Exception ex)
                {
                    throw new SkillProcessingException((row.ElementAtOrDefault(config.Skills.Name) ?? string.Empty).ToString(), ex);
                }
            }

            this.TerrainTypes = new Dictionary<string, TerrainType>();
            foreach (IList<object> row in config.TerrainTypes.Query.Data)
            {
                try
                {
                    IList<string> type = row.Select(r => r.ToString()).ToList();
                    if (string.IsNullOrEmpty(type.ElementAtOrDefault<string>(config.TerrainTypes.Name)))
                        continue;
                    this.TerrainTypes.Add(type.ElementAtOrDefault<string>(config.TerrainTypes.Name), new TerrainType(config.TerrainTypes, type));
                }
                catch (Exception ex)
                {
                    throw new TerrainTypeProcessingException((row.ElementAtOrDefault(config.TerrainTypes.Name) ?? string.Empty).ToString(), ex);
                }
            }
        }

        public void RemoveUnusedObjects()
        {
            //Cull unused classes
            foreach (string key in this.Classes.Keys.ToList())
                if (!this.Classes[key].Matched)
                    this.Classes.Remove(key);

            //Cull unused items
            foreach (string key in this.Items.Keys.ToList())
                if (!this.Items[key].Matched)
                    this.Items.Remove(key);

            //Cull unused skills
            foreach (string key in this.Skills.Keys.ToList())
                if (!this.Skills[key].Matched)
                    this.Skills.Remove(key);

            //Cull unused terrain types
            foreach (string key in this.TerrainTypes.Keys.ToList())
                if (!this.TerrainTypes[key].Matched)
                    this.TerrainTypes.Remove(key);
        }
    }
}
