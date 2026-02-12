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
    #region Interface

    /// <inheritdoc cref="BattleStyle"/>
    public interface IBattleStyle : IMatchable
    {
        /// <inheritdoc cref="BattleStyle.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="BattleStyle.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class BattleStyle : Matchable, IBattleStyle
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the battle style.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the battle style's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public BattleStyle(BattleStylesConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IBattleStyle</c> from each valid row.
        /// </summary>
        /// <exception cref="BattleStyleProcessingException"></exception>
        public static IDictionary<string, IBattleStyle> BuildDictionary(BattleStylesConfig config)
        {
            IDictionary<string, IBattleStyle> battleStyles = new Dictionary<string, IBattleStyle>();
            if (config?.Queries is null) return battleStyles;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
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
        /// Matches each string in <paramref name="names"/> to an <c>IBattleStyle</c> in <paramref name="battleStyles"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IBattleStyle> MatchNames(IDictionary<string, IBattleStyle> battleStyles, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(battleStyles, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IBattleStyle</c> in <paramref name="battleStyles"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedBattleStyleException"></exception>
        public static IBattleStyle MatchName(IDictionary<string, IBattleStyle> battleStyles, string name, bool flagAsMatched = true)
        {
            IBattleStyle match;
            if (!battleStyles.TryGetValue(name, out match))
                throw new UnmatchedBattleStyleException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}