using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Tag
    {
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
        /// Constructor.
        /// </summary>
        public Tag(TagConfig config, IList<string> data)
        {
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", false);
            this.ShowOnUnit = (   ParseHelper.SafeStringParse(data, config.ShowOnUnit, "Show On Unit", false) == "Yes" 
                               && !string.IsNullOrEmpty(this.SpriteURL) );
        }
    }
}
