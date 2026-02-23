using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Match;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Adjutants;
using RedditEmblemAPI.Helpers;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="Adjutant"/>
    public interface IAdjutant : IMatchable
    {
        /// <inheritdoc cref="Adjutant.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Adjutant.CombatStatModifiers"/>
        IDictionary<string, int> CombatStatModifiers { get; set; }

        /// <inheritdoc cref="Adjutant.StatModifiers"/>
        IDictionary<string, int> StatModifiers { get; set; }

        /// <inheritdoc cref="Adjutant.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class Adjutant : Matchable, IAdjutant
    {
        #region Attributes

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
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.CombatStatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.CombatStatModifiers, data, false);
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IAdjutant</c> from each valid row.
        /// </summary>
        /// <exception cref="AdjutantProcessingException"></exception>
        public static IDictionary<string, IAdjutant> BuildDictionary(AdjutantsConfig config)
        {
            IDictionary<string, IAdjutant> adjutants = new Dictionary<string, IAdjutant>();
            if (config?.Queries is null) return adjutants;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
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
        /// Matches each string in <paramref name="names"/> to an <c>IAdjutant</c> in <paramref name="adjutants"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched</c> for all returned objects.</param>
        public static List<IAdjutant> MatchNames(IDictionary<string, IAdjutant> adjutants, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(adjutants, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IAdjutant</c> in <paramref name="adjutants"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched</c> for the returned object.</param>
        /// <exception cref="UnmatchedAdjutantException"></exception>
        public static IAdjutant MatchName(IDictionary<string, IAdjutant> adjutants, string name, bool flagAsMatched = true)
        {
            IAdjutant match;
            if (!adjutants.TryGetValue(name, out match))
                throw new UnmatchedAdjutantException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
