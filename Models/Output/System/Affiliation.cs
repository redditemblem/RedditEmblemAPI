using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Affiliations;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Affiliation : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this affiliation was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

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
        /// The sprite image URL for the affiliation.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Flag indicating whether or not all units belonging to this affiliation should have their sprites flipped.
        /// </summary>
        public bool FlipUnitSprites { get; set; }

        /// <summary>
        /// List of the affiliation's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Affiliation(AffiliationsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.Grouping = DataParser.Int_NonZeroPositive(data, config.Grouping, "Grouping");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.FlipUnitSprites = DataParser.OptionalBoolean_YesNo(data, config.FlipUnitSprites, "Flip Unit Sprites");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Affiliation</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Affiliation</c> from each valid row.
        /// </summary>
        /// <exception cref="AffiliationProcessingException"></exception>
        public static IDictionary<string, Affiliation> BuildDictionary(AffiliationsConfig config)
        {
            IDictionary<string, Affiliation> affiliations = new Dictionary<string, Affiliation>();
            if (config == null || config.Queries == null)
                return affiliations;

            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> aff = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(aff, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!affiliations.TryAdd(name, new Affiliation(config, aff)))
                        throw new NonUniqueObjectNameException("affiliation");
                }
                catch (Exception ex)
                {
                    throw new AffiliationProcessingException(name, ex);
                }
            }

            return affiliations;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>Affiliation</c> in <paramref name="affiliations"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Affiliation> MatchNames(IDictionary<string, Affiliation> affiliations, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(affiliations, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>Affiliation</c> in <paramref name="affiliations"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedAffiliationException"></exception>
        public static Affiliation MatchName(IDictionary<string, Affiliation> affiliations, string name, bool skipMatchedStatusSet = false)
        {
            Affiliation match;
            if (!affiliations.TryGetValue(name, out match))
                throw new UnmatchedAffiliationException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion
    }
}