using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.BattleStyles;
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
    public class BattleStyle : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this battle style was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The battle style's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite image URL for the battle style.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the battle style's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public BattleStyle(BattleStylesConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>BattleStyle</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>BattleStyle</c> from each valid row.
        /// </summary>
        /// <exception cref="BattleStyleProcessingException"></exception>
        public static IDictionary<string, BattleStyle> BuildDictionary(BattleStylesConfig config)
        {
            IDictionary<string, BattleStyle> battleStyles = new Dictionary<string, BattleStyle>();
            if (config == null || config.Query == null)
                return battleStyles;

            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> battleStyle = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(battleStyle, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!battleStyles.TryAdd(name, new BattleStyle(config, battleStyle)))
                        throw new NonUniqueObjectNameException("battle style");
                }
                catch (Exception ex)
                {
                    throw new BattleStyleProcessingException(name, ex);
                }
            }

            return battleStyles;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>BattleStyle</c> in <paramref name="battleStyles"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<BattleStyle> MatchNames(IDictionary<string, BattleStyle> battleStyles, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(battleStyles, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Class</c> in <paramref name="battleStyles"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedBattleStyleException"></exception>
        public static BattleStyle MatchName(IDictionary<string, BattleStyle> battleStyles, string name, bool skipMatchedStatusSet = false)
        {
            BattleStyle match;
            if (!battleStyles.TryGetValue(name, out match))
                throw new UnmatchedBattleStyleException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion
    }
}