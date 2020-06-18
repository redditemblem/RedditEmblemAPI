using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Affiliation
    {
        /// <summary>
        /// Flag indicating whether or not this affiliation was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the affiliation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The grouping that the affiliation belongs to.
        /// </summary>
        [JsonIgnore]
        public int Grouping { get; set; }

        /// <summary>
        /// List of the affiliation's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="config"></param>
        public Affiliation(AffiliationsConfig config, IList<string> data)
        {
            this.Matched = false;
            this.Name = data.ElementAtOrDefault<string>(config.Name).Trim();
            this.Grouping = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Grouping), "Grouping", true);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }
    }
}
