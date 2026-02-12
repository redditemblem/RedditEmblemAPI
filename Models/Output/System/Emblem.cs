using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
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

    /// <inheritdoc cref="Emblem"/>
    public interface IEmblem : IMatchable
    {
        /// <inheritdoc cref="Emblem.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Emblem.Tagline"/>
        public string Tagline { get; set; }

        /// <inheritdoc cref="Emblem.EngagedUnitAura"/>
        string EngagedUnitAura { get; set; }

        /// <inheritdoc cref="Emblem.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class Emblem : Matchable, IEmblem
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the emblem.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The emblem's tagline.
        /// </summary>
        public string Tagline { get; set; }

        /// <summary>
        /// The aura color applied to units engaged with the emblem.
        /// </summary>
        [JsonIgnore]
        public string EngagedUnitAura { get; set; }

        /// <summary>
        /// List of the emblem's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        public Emblem(EmblemsConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Tagline = DataParser.OptionalString(data, config.Tagline, "Tagline");
            this.EngagedUnitAura = DataParser.OptionalString_HexCode(data, config.EngagedUnitAura, "Engaged Unit Aura");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IEmblem</c> from each valid row.
        /// </summary>
        /// <exception cref="EmblemProcessingException"></exception>
        public static IDictionary<string, IEmblem> BuildDictionary(EmblemsConfig config)
        {
            IDictionary<string, IEmblem> emblems = new Dictionary<string, IEmblem>();
            if (config?.Queries is null) return emblems;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> emblem = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(emblem, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!emblems.TryAdd(name, new Emblem(config, emblem)))
                        throw new NonUniqueObjectNameException("emblem");
                }
                catch (Exception ex)
                {
                    throw new EmblemProcessingException(name, ex);
                }
            }

            return emblems;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>IEmblem</c> in <paramref name="emblems"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IEmblem> MatchNames(IDictionary<string, IEmblem> emblems, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(emblems, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IEmblem</c> in <paramref name="emblems"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedEmblemException"></exception>
        public static IEmblem MatchName(IDictionary<string, IEmblem> emblems, string name, bool flagAsMatched = true)
        {
            IEmblem match;
            if (!emblems.TryGetValue(name, out match))
                throw new UnmatchedEmblemException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
