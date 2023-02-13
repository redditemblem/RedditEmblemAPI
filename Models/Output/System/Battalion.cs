using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
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
    /// Object representing a battalion definition in the team's system. 
    /// </summary>
    public class Battalion : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this battalion was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The battalion's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The battalion's gambit.
        /// </summary>
        [JsonIgnore]
        public Gambit GambitObj { get; set; }

        /// <summary>
        /// The battalion's icon sprite URL.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The battalion's max endurance.
        /// </summary>
        public int MaxEndurance { get; set; }

        /// <summary>
        /// The authority (or other) rank required to use the battalion.
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// Collection of stat values for the battalion. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Collection of stat modifiers that will be applied when a unit equips this battalion.
        /// </summary>
        public IDictionary<string, int> StatModifiers { get; set; }

        /// <summary>
        /// Any text information about the battalion to display.
        /// </summary>
        public List<string> TextFields { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// For JSON serialization only. The name of the <c>GambitObj</c>.
        /// </summary>
        [JsonProperty]
        private string Gambit { get { return GambitObj?.Name; } }

        #endregion JSON Serialization Only

        #endregion

        public Battalion(BattalionsConfig config, IEnumerable<string> data, IDictionary<string, Gambit> gambits)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");

            string gambitName = DataParser.String(data, config.Gambit, "Gambit");
            this.GambitObj = System.Gambit.MatchName(gambits, gambitName, true);

            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxEndurance = DataParser.Int_Positive(data, config.MaxEndurance, "Max Endurance");
            this.Rank = DataParser.OptionalString(data, config.Rank, "Rank");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            BuildStats(config.Stats, data);
            BuildStatModifiers(config.StatModifiers, data);
        }

        private void BuildStats(List<NamedStatConfig> configs, IEnumerable<string> data)
        {
            this.Stats = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName);
                this.Stats.Add(stat.SourceName, val);
            }
        }

        private void BuildStatModifiers(List<NamedStatConfig> configs, IEnumerable<string> data)
        {
            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, $"{stat.SourceName} Modifier");
                if (val == 0) continue;
                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        public int MatchStatName(string name)
        {
            int value;
            if (!this.Stats.TryGetValue(name, out value))
                throw new UnmatchedStatException(name);

            return value;
        }

        #region Static Functions

        public static IDictionary<string, Battalion> BuildDictionary(BattalionsConfig config, IDictionary<string, Gambit> gambits)
        {
            IDictionary<string, Battalion> battalions = new Dictionary<string, Battalion>();
            if (config == null || config.Query == null)
                return battalions;

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    IEnumerable<string> battalion = row.Select(r => r.ToString());
                    string name = DataParser.OptionalString(battalion, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!battalions.TryAdd(name, new Battalion(config, battalion, gambits)))
                        throw new NonUniqueObjectNameException("battalion");
                }
                catch (Exception ex)
                {
                    throw new BattalionProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return battalions;
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Battalion</c> in <paramref name="battalions"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Battalion> MatchNames(IDictionary<string, Battalion> battalions, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(battalions, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Battalion</c> in <paramref name="battalions"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedBattalionException"></exception>
        public static Battalion MatchName(IDictionary<string, Battalion> battalions, string name, bool skipMatchedStatusSet = false)
        {
            Battalion match;
            if (!battalions.TryGetValue(name, out match))
                throw new UnmatchedBattalionException(name);

            if (!skipMatchedStatusSet)
            {
                match.Matched = true;
                match.GambitObj.Matched = true;
            }

            return match;
        }

        #endregion

    }
}
