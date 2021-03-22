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

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Tag(TagConfig config, IList<string> data)
        {
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", false);
            this.ShowOnUnit = (   ParseHelper.SafeStringParse(data, config.ShowOnUnit, "Show On Unit", false) == "Yes" 
                               && !string.IsNullOrEmpty(this.SpriteURL) );
        }

        #region Static Functions

        public static IDictionary<string, Tag> BuildDictionary(TagConfig config)
        {
            IDictionary<string, Tag> tags = new Dictionary<string, Tag>();

            foreach (IList<object> row in config.Query.Data)
            {
                try
                {
                    IList<string> tag = row.Select(r => r.ToString()).ToList();
                    string name = ParseHelper.SafeStringParse(tag, config.Name, "Name", false);
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
