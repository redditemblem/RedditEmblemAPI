using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Tag
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this status was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the tag.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Flag indicating if the tag's sprite should be displayed over the map sprite of units who possess it.
        /// </summary>
        public bool ShowOnUnit { get; set; }

        /// <summary>
        /// The unit aura color for the tag.
        /// </summary>
        [JsonIgnore]
        public string UnitAura { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tag(TagConfig config, List<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.ShowOnUnit = (DataParser.OptionalBoolean_YesNo(data, config.ShowOnUnit, "Show On Unit") && !string.IsNullOrEmpty(this.SpriteURL));
            this.UnitAura = DataParser.OptionalString_HexCode(data, config.UnitAura, "Unit Aura");
        }

        #region Static Functions

        public static IDictionary<string, Tag> BuildDictionary(TagConfig config)
        {
            IDictionary<string, Tag> tags = new Dictionary<string, Tag>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> tag = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(tag, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!tags.TryAdd(name, new Tag(config, tag)))
                        throw new NonUniqueObjectNameException("tag");
                }
                catch (Exception ex)
                {
                    throw new TagProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return tags;
        }

        #endregion
    }
}