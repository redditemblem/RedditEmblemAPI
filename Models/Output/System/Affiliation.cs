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
    #region Interface

    /// <inheritdoc cref="Affiliation"/>
    public interface IAffiliation : IMatchable
    {
        /// <inheritdoc cref="Affiliation.Grouping"/>
        int Grouping {  get; set; }

        /// <inheritdoc cref="Affiliation.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Affiliation.FlipUnitSprites"/>
        bool FlipUnitSprites { get; set; }

        /// <inheritdoc cref="Affiliation.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface
    public class Affiliation : Matchable, IAffiliation
    {
        #region Attributes

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

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Affiliation(AffiliationsConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.Grouping = DataParser.Int_NonZeroPositive(data, config.Grouping, "Grouping");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.FlipUnitSprites = DataParser.OptionalBoolean_YesNo(data, config.FlipUnitSprites, "Flip Unit Sprites");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>IAffiliation</c> from each valid row.
        /// </summary>
        /// <exception cref="AffiliationProcessingException"></exception>
        public static IDictionary<string, IAffiliation> BuildDictionary(AffiliationsConfig config)
        {
            IDictionary<string, IAffiliation> affiliations = new Dictionary<string, IAffiliation>();
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
        /// Matches each of the strings in <paramref name="names"/> to an <c>IAffiliation</c> in <paramref name="affiliations"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IAffiliation> MatchNames(IDictionary<string, IAffiliation> affiliations, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(affiliations, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IAffiliation</c> in <paramref name="affiliations"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedAffiliationException"></exception>
        public static IAffiliation MatchName(IDictionary<string, IAffiliation> affiliations, string name, bool flagAsMatched = true)
        {
            IAffiliation match;
            if (!affiliations.TryGetValue(name, out match))
                throw new UnmatchedAffiliationException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}