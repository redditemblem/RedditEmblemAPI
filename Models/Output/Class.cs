﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Classes;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
{
    public class Class
    {
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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="data"></param>
        public Class(ClassesConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = data.ElementAtOrDefault(config.Name).Trim();
            this.MovementType = data.ElementAtOrDefault<string>(config.MovementType).Trim();

            this.Tags = ParseHelper.StringCSVParse(data, config.Tags);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }

    }
}
