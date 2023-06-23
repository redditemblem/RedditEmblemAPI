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
    public class EngageAttack : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this engage attack was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The engage attack's name. 
        /// </summary>
        public string Name { get; set; }

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
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds an <c>EngageAttack</c> from each valid row.
        /// </summary>
        /// <exception cref="EngageAttackProcessingException"></exception>
        public static IDictionary<string, EngageAttack> BuildDictionary(EngageAttacksConfig config)
        {
            IDictionary<string, EngageAttack> engageAttacks = new Dictionary<string, EngageAttack>();
            if (config == null || config.Query == null)
                return engageAttacks;

            foreach (List<object> row in config.Query.Data)
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
        /// Matches each of the strings in <paramref name="names"/> to an <c>EngageAttack</c> in <paramref name="engageAttacks"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<EngageAttack> MatchNames(IDictionary<string, EngageAttack> engageAttacks, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(engageAttacks, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>EngageAttack</c> in <paramref name="engageAttacks"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedEngageAttackException"></exception>
        public static EngageAttack MatchName(IDictionary<string, EngageAttack> engageAttacks, string name, bool skipMatchedStatusSet = false)
        {
            EngageAttack match;
            if (!engageAttacks.TryGetValue(name, out match))
                throw new UnmatchedEngageAttackException(name);

            if (!skipMatchedStatusSet) match.Matched = true;

            return match;
        }

        #endregion Static Functions
    }
}