using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Adjutants;

namespace RedditEmblemAPI.Models.Output.System
{
    public class Adjutant : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this adjutant was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the adjutant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the adjutant.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Collection of combat stat modifiers that will be applied to the owning unit when this adjutant is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied to the owning unit when this adjutant is equipped.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of the adjutant's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Adjutant(AdjutantsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data, false);
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Adjutant</c> from each valid row.
        /// </summary>
        /// <exception cref="AdjutantProcessingException"></exception>
        public static IDictionary<string, Adjutant> BuildDictionary(AdjutantsConfig config)
        {
            IDictionary<string, Adjutant> adjutants = new Dictionary<string, Adjutant>();
            if (config == null || config.Query == null)
                return adjutants;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> adjutant = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(adjutant, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!adjutants.TryAdd(name, new Adjutant(config, adjutant)))
                        throw new NonUniqueObjectNameException("adjutant");
                }
                catch (Exception ex)
                {
                    throw new AdjutantProcessingException(name, ex);
                }
            }

            return adjutants;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>Adjutant</c> in <paramref name="adjutants"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Adjutant> MatchNames(IDictionary<string, Adjutant> adjutants, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(adjutants, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>Adjutant</c> in <paramref name="adjutants"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedAdjutantException"></exception>
        public static Adjutant MatchName(IDictionary<string, Adjutant> adjutants, string name, bool skipMatchedStatusSet = false)
        {
            Adjutant match;
            if (!adjutants.TryGetValue(name, out match))
                throw new UnmatchedAdjutantException(name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
        }

        #endregion
    }
}
