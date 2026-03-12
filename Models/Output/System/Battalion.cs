using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
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

    /// <inheritdoc cref="Battalion"/>
    public interface IBattalion : IMatchable
    {
        /// <inheritdoc cref="Battalion.Gambit"/>
        IGambit Gambit { get; set; }

        /// <inheritdoc cref="Battalion.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Battalion.MaxEndurance"/>
        int MaxEndurance { get; set; }

        /// <inheritdoc cref="Battalion.Rank"/>
        string Rank { get; set; }

        /// <inheritdoc cref="Battalion.Stats"/>
        IDictionary<string, int> Stats { get; set; }

        /// <inheritdoc cref="Battalion.StatModifiers"/>
        IDictionary<string, int> StatModifiers { get; set; }

        /// <inheritdoc cref="Battalion.TextFields"/>
        List<string> TextFields { get; set; }

        /// <inheritdoc cref="Battalion.MatchStatName(string)"/>
        int MatchStatName(string name);
    }

    #endregion Interface

    /// <summary>
    /// Object representing a battalion definition in the team's system. 
    /// </summary>
    public class Battalion : Matchable, IBattalion
    {
        #region Attributes

        /// <summary>
        /// The battalion's gambit.
        /// </summary>
        [JsonConverter(typeof(MatchableNameConverter))]
        public IGambit Gambit { get; set; }

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

        #endregion Attributes

        public Battalion(BattalionsConfig config, IEnumerable<string> data, IDictionary<string, IGambit> gambits)
        {
            this.Name = DataParser.String(data, config.Name, "Name");

            string gambitName = DataParser.String(data, config.Gambit, "Gambit");
            this.Gambit = System.Gambit.MatchName(gambits, gambitName, false);

            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxEndurance = DataParser.Int_Positive(data, config.MaxEndurance, "Max Endurance");
            this.Rank = DataParser.OptionalString(data, config.Rank, "Rank");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Stats = DataParser.NamedStatDictionary_OptionalInt_Any(config.Stats, data, true);
            this.StatModifiers = DataParser.NamedStatDictionary_OptionalInt_Any(config.StatModifiers, data, false, "{0} Modifier");
        }

        /// <summary>
        /// Searches <c>this.Stats</c> for a stat matching <paramref name="name"/>. If one is found, returns its value.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public int MatchStatName(string name)
        {
            int value;
            if (!this.Stats.TryGetValue(name, out value))
                throw new UnmatchedStatException(name);

            return value;
        }

        public override void FlagAsMatched()
        {
            this.Matched = true;
            this.Gambit.FlagAsMatched();
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IBattalion</c> from each valid row.
        /// </summary>
        /// <exception cref="BattalionProcessingException"></exception>
        public static IDictionary<string, IBattalion> BuildDictionary(BattalionsConfig config, IDictionary<string, IGambit> gambits)
        {
            IDictionary<string, IBattalion> battalions = new Dictionary<string, IBattalion>();
            if (config?.Queries is null) return battalions;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> battalion = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(battalion, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!battalions.TryAdd(name, new Battalion(config, battalion, gambits)))
                        throw new NonUniqueObjectNameException("battalion");
                }
                catch (Exception ex)
                {
                    throw new BattalionProcessingException(name, ex);
                }
            }

            return battalions;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>IBattalion</c> in <paramref name="battalions"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IBattalion> MatchNames(IDictionary<string, IBattalion> battalions, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(battalions, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IBattalion</c> in <paramref name="battalions"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedBattalionException"></exception>
        public static IBattalion MatchName(IDictionary<string, IBattalion> battalions, string name, bool flagAsMatched = true)
        {
            IBattalion match;
            if (!battalions.TryGetValue(name, out match))
                throw new UnmatchedBattalionException(name);

            if (flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions

    }
}
