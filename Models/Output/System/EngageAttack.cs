using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    #region Interface

    /// <inheritdoc cref="EngageAttack"/>
    public interface IEngageAttack : IMatchable
    {
        /// <inheritdoc cref="EngageAttack.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="EngageAttack.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    public class EngageAttack : Matchable, IEngageAttack
    {
        #region Attributes

        /// <summary>
        /// The sprite image URL for the engage attack.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// List of the engage attack's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        public EngageAttack(EngageAttacksConfig config, IEnumerable<string> data) 
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IEngageAttack</c> from each valid row.
        /// </summary>
        /// <exception cref="EngageAttackProcessingException"></exception>
        public static IDictionary<string, IEngageAttack> BuildDictionary(EngageAttacksConfig config)
        {
            IDictionary<string, IEngageAttack> engageAttacks = new Dictionary<string, IEngageAttack>();
            if (config?.Queries is null) return engageAttacks;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> attack = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(attack, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!engageAttacks.TryAdd(name, new EngageAttack(config, attack)))
                        throw new NonUniqueObjectNameException("engage attack");
                }
                catch (Exception ex)
                {
                    throw new EngageAttackProcessingException(name, ex);
                }
            }

            return engageAttacks;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>IEngageAttack</c> in <paramref name="engageAttacks"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IEngageAttack> MatchNames(IDictionary<string, IEngageAttack> engageAttacks, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(engageAttacks, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IEngageAttack</c> in <paramref name="engageAttacks"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedEngageAttackException"></exception>
        public static IEngageAttack MatchName(IDictionary<string, IEngageAttack> engageAttacks, string name, bool flagAsMatched = true)
        {
            IEngageAttack match;
            if (!engageAttacks.TryGetValue(name, out match))
                throw new UnmatchedEngageAttackException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}