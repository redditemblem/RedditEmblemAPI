using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Interfaces;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public bool Matched { get; private set; }

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

        public Battalion(BattalionsConfig config, IEnumerable<string> data, IReadOnlyDictionary<string, Gambit> gambits)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");

            string gambitName = DataParser.String(data, config.Gambit, "Gambit");
            this.GambitObj = System.Gambit.MatchName(gambits, gambitName, true);

            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxEndurance = DataParser.Int_Positive(data, config.MaxEndurance, "Max Endurance");
            this.Rank = DataParser.OptionalString(data, config.Rank, "Rank");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Stats = DataParser.NamedStatDictionary_OptionalInt_Any(config.Stats, data, true);
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false, "{0} Modifier");
        }

        public int MatchStatName(string name)
        {
            int value;
            if (!this.Stats.TryGetValue(name, out value))
                throw new UnmatchedStatException(name);

            return value;
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Battalion</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
            this.GambitObj.FlagAsMatched();
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Battalion</c> from each valid row.
        /// </summary>
        /// <exception cref="BattalionProcessingException"></exception>
        public static IReadOnlyDictionary<string, Battalion> BuildDictionary(BattalionsConfig config, IReadOnlyDictionary<string, Gambit> gambits)
        {
            ConcurrentDictionary<string, Battalion> battalions = new ConcurrentDictionary<string, Battalion>();
            if (config == null || config.Queries == null)
                return battalions.ToFrozenDictionary();

            try
            {
                Parallel.ForEach(config.Queries.SelectMany(q => q.Data), row =>
                {
                    string name = string.Empty;
                    try
                    {
                        IEnumerable<string> battalion = row.Select(r => r.ToString());
                        name = DataParser.OptionalString(battalion, config.Name, "Name");
                        if (string.IsNullOrEmpty(name)) return;

                        if (!battalions.TryAdd(name, new Battalion(config, battalion, gambits)))
                            throw new NonUniqueObjectNameException("battalion");
                    }
                    catch (Exception ex)
                    {
                        throw new BattalionProcessingException(name, ex);
                    }
                });
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
            
            return battalions.ToFrozenDictionary();
        }

        /// <summary>
        /// Matches each of the strings in <paramref name="names"/> to a <c>Battalion</c> in <paramref name="battalions"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Battalion> MatchNames(IReadOnlyDictionary<string, Battalion> battalions, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(battalions, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Battalion</c> in <paramref name="battalions"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedBattalionException"></exception>
        public static Battalion MatchName(IReadOnlyDictionary<string, Battalion> battalions, string name, bool skipMatchedStatusSet = false)
        {
            Battalion match;
            if (!battalions.TryGetValue(name, out match))
                throw new UnmatchedBattalionException(name);

            if (!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion

    }
}
