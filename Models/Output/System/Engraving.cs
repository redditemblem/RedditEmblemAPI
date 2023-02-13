using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Items;
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
    /// <summary>
    /// Object representing an item engraving definition in the team's system.
    /// </summary>
    public class Engraving : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this engraving was found on an item. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The engraving's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the engraving.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// Dictionary of the stat modifiers this engraving applies to items.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// List of the engraving's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public Engraving(EngravingsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            BuildStatModifiers(config.StatModifiers, data);
        }

        public void BuildStatModifiers(List<NamedStatConfig> configs, IEnumerable<string> data)
        {
            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, $"{stat.SourceName} Modifier");
                if (val == 0) continue;
                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        #region Static Functions

        public static IDictionary<string, Engraving> BuildDictionary(EngravingsConfig config)
        {
            IDictionary<string, Engraving> engravings = new Dictionary<string, Engraving>();
            if (config == null || config.Query == null)
                return engravings;

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    IEnumerable<string> engraving = row.Select(r => r.ToString());
                    string name = DataParser.OptionalString(engraving, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!engravings.TryAdd(name, new Engraving(config, engraving)))
                        throw new NonUniqueObjectNameException("engraving");
                }
                catch (Exception ex)
                {
                    throw new EngravingProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return engravings;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to an <c>Engraving</c> in <paramref name="engravings"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Engraving> MatchNames(IDictionary<string, Engraving> engravings, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(engravings, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>Engraving</c> in <paramref name="engravings"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedEngravingException"></exception>
        public static Engraving MatchName(IDictionary<string, Engraving> engravings, string name, bool skipMatchedStatusSet = false)
        {
            Engraving match;
            if (!engravings.TryGetValue(name, out match))
                throw new UnmatchedEngravingException(name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
        }

        #endregion Static Functions
    }
}
