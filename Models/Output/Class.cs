using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class Class
    {
        public Class()
        {
            this.Matched = false;

            this.Tags = new List<string>();
            this.TextFields = new List<string>();
        }

        /// <summary>
        /// Flag indicating whether or not this class was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The class's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The class's movement type.
        /// </summary>
        [JsonIgnore]
        public string MovementType { get; set; }

        /// <summary>
        /// List of the class's tags.
        /// </summary>
        [JsonIgnore]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// List of the class's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }
    }
}
