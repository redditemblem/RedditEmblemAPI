using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System;
using RedditEmblemAPI.Models.Configuration.System.CombatArts;
using System.Linq;
using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Output.System
{
    public class CombatArt : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this combat art was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The combat art's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the combat art.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The minimum weapon rank required to wield the combat art, if applicable.
        /// </summary>
        public string WeaponRank { get; set; }

        /// <summary>
        /// The combat art's category. (ex. Sword)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The unit stats that the combat art uses in calculations, if applicable.
        /// </summary>
        public List<string> UtilizedStats { get; set; }

        /// <summary>
        /// Container object for the combat art's range values.
        /// </summary>
        public CombatArtRange Range { get; set; }

        /// <summary>
        /// Collection of stat values for the combat art. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// The weapon durability cost to use the combat art.
        /// </summary>
        public int DurabilityCost { get; set; }

        /// <summary>
        /// The combat art's tags.
        /// </summary>
        [JsonIgnore]
        public List<Tag> TagsList { get; set; }

        /// <summary>
        /// List of the combat art's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// For JSON serialization only. The combat art's tags.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private IEnumerable<string> Tags { get { return this.TagsList.Any() ? this.TagsList.Select(t => t.Name) : null; } }

        #endregion JSON Serialization Only

        #endregion Attributes

        public CombatArt(CombatArtsConfig config, IEnumerable<string> data, IDictionary<string, Tag> tags)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.WeaponRank = DataParser.OptionalString(data, config.WeaponRank, "Weapon Rank");
            this.Category = DataParser.OptionalString(data, config.Category, "Category");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.Range = new CombatArtRange(config.Range, data);
            this.Stats = DataParser.NamedStatDictionary_OptionalInt_Any(config.Stats, data, true);
            this.DurabilityCost = DataParser.OptionalInt_Any(data, config.DurabilityCost, "Durability Cost");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            IEnumerable<string> tagList = DataParser.List_StringCSV(data, config.Tags).Distinct();
            this.TagsList = Tag.MatchNames(tags, tagList, true);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>CombatArt</c> from each valid row.
        /// </summary>
        /// <exception cref="CombatArtProcessingException"></exception>
        public static IDictionary<string, CombatArt> BuildDictionary(CombatArtsConfig config, IDictionary<string, Tag> tags)
        {
            IDictionary<string, CombatArt> combatArts = new Dictionary<string, CombatArt>();
            if (config == null || config.Query == null)
                return combatArts;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> combatArt = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(combatArt, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!combatArts.TryAdd(name, new CombatArt(config, combatArt, tags)))
                        throw new NonUniqueObjectNameException("combat art");
                }
                catch (Exception ex)
                {
                    throw new CombatArtProcessingException(name, ex);
                }
            }

            return combatArts;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>CombatArt</c> in <paramref name="combatArts"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<CombatArt> MatchNames(IDictionary<string, CombatArt> combatArts, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(combatArts, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>CombatArt</c> in <paramref name="combatArts"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedCombatArtException"></exception>
        public static CombatArt MatchName(IDictionary<string, CombatArt> combatArts, string name, bool skipMatchedStatusSet = false)
        {
            CombatArt match;
            if (!combatArts.TryGetValue(name, out match))
                throw new UnmatchedCombatArtException(name);

            if (!skipMatchedStatusSet)
            {
                match.Matched = true;
                foreach (Tag tag in match.TagsList)
                    tag.Matched = true;
            }

            return match;
        }

        #endregion
    }
}
