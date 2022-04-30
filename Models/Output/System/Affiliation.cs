using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Affiliation
    {
        #region Attributes

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
        public List<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Affiliation(AffiliationsConfig config, List<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.Grouping = DataParser.Int_NonZeroPositive(data, config.Grouping, "Grouping");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        public static IDictionary<string, Affiliation> BuildDictionary(AffiliationsConfig config)
        {
            IDictionary<string, Affiliation> affiliations = new Dictionary<string, Affiliation>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> aff = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(aff, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!affiliations.TryAdd(name, new Affiliation(config, aff)))
                        throw new NonUniqueObjectNameException("affiliation");
                }
                catch (Exception ex)
                {
                    throw new AffiliationProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return affiliations;
        }

        #endregion
    }
}
