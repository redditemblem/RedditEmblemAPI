using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output
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

            int grouping;
            if (!int.TryParse(data.ElementAtOrDefault<string>(config.Grouping), out grouping) || grouping <= 0)
                throw new PositiveIntegerException("Grouping", data.ElementAtOrDefault<string>(config.Grouping));
            this.Grouping = grouping;

            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }
    }
}
