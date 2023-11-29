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
    public class Emblem : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this emblem was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The emblem's name. 
        /// </summary>
        public string Name { get; set; }

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
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.Tagline = DataParser.OptionalString(data, config.Tagline, "Tagline");
            this.EngagedUnitAura = DataParser.OptionalString_HexCode(data, config.EngagedUnitAura, "Engaged Unit Aura");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Emblem</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>Emblem</c> from each valid row.
        /// </summary>
        /// <exception cref="EmblemProcessingException"></exception>
        public static IDictionary<string, Emblem> BuildDictionary(EmblemsConfig config)
        {
            IDictionary<string, Emblem> emblems = new Dictionary<string, Emblem>();
            if (config == null || config.Query == null)
                return emblems;

            foreach (List<object> row in config.Query.Data)
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
        /// Matches each of the strings in <paramref name="names"/> to an <c>Emblem</c> in <paramref name="emblems"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Emblem> MatchNames(IDictionary<string, Emblem> emblems, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(emblems, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Emblem</c> in <paramref name="emblems"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedEmblemException"></exception>
        public static Emblem MatchName(IDictionary<string, Emblem> emblems, string name, bool skipMatchedStatusSet = false)
        {
            Emblem match;
            if (!emblems.TryGetValue(name, out match))
                throw new UnmatchedEmblemException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
