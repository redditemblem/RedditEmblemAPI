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

    /// <inheritdoc cref="Gambit"/>
    public interface IGambit : IMatchable
    {
        /// <inheritdoc cref="Gambit.SpriteURL"/>
        string SpriteURL { get; set; }

        /// <inheritdoc cref="Gambit.MaxUses"/>
        int MaxUses { get; set; }

        /// <inheritdoc cref="Gambit.UtilizedStats"/>
        List<string> UtilizedStats { get; set; }

        /// <inheritdoc cref="Gambit.Range"/>
        IGambitRange Range { get; set; }

        /// <inheritdoc cref="Gambit.Stats"/>
        IDictionary<string, int> Stats { get; set; }

        /// <inheritdoc cref="Gambit.TextFields"/>
        List<string> TextFields { get; set; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a gambit definition in the team's system. 
    /// </summary>
    public class Gambit : Matchable, IGambit
    {
        #region Attributes

        /// <summary>
        /// The battalion's icon sprite URL.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The gambit's max number of uses.
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// The unit stats (ex. Str/Mag/etc) that the gambit uses.
        /// </summary>
        public List<string> UtilizedStats { get; set; }

        /// <summary>
        /// Container object for the gambit's range values.
        /// </summary>
        public IGambitRange Range { get; set; }

        /// <summary>
        /// Collection of stat values for the gambit. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Any text information about the gambit to display.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion Attributes

        public Gambit(GambitsConfig config, IEnumerable<string> data)
        {
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxUses = DataParser.Int_Positive(data, config.MaxUses, "Max Uses");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Range = new GambitRange(config.Range, data);
            this.Stats = DataParser.NamedStatDictionary_Int_Any(config.Stats, data, true);
        }

        #region Static Functions

        /// <summary>
        /// Iterates through <paramref name="config"/>'s queried data and builds an <c>IGambit</c> from each valid row.
        /// </summary>
        /// <exception cref="GambitProcessingException"></exception>
        public static IDictionary<string, IGambit> BuildDictionary(GambitsConfig config)
        {
            IDictionary<string, IGambit> gambits = new Dictionary<string, IGambit>();
            if (config?.Queries is null) return gambits;

            foreach (IList<object> row in config.Queries.SelectMany(q => q.Data))
            {
                string name = string.Empty;
                try
                {
                    IEnumerable<string> gambit = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(gambit, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!gambits.TryAdd(name, new Gambit(config, gambit)))
                        throw new NonUniqueObjectNameException("gambit");
                }
                catch (Exception ex)
                {
                    throw new GambitProcessingException(name, ex);
                }
            }

            return gambits;
        }

        /// <summary>
        /// Matches each string in <paramref name="names"/> to an <c>IGambit</c> in <paramref name="gambits"/> and returns the matches as a list.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for all returned objects.</param>
        public static List<IGambit> MatchNames(IDictionary<string, IGambit> gambits, IEnumerable<string> names, bool flagAsMatched = true)
        {
            return names.Select(n => MatchName(gambits, n, flagAsMatched)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to an <c>IGambit</c> in <paramref name="gambits"/> and returns it.
        /// </summary>
        /// <param name="flagAsMatched">If true, calls <c>IMatchable.FlagAsMatched()</c> for the returned object.</param>
        /// <exception cref="UnmatchedGambitException"></exception>
        public static IGambit MatchName(IDictionary<string, IGambit> gambits, string name, bool flagAsMatched = true)
        {
            IGambit match;
            if (!gambits.TryGetValue(name, out match))
                throw new UnmatchedGambitException(name);
            
            if(flagAsMatched) match.FlagAsMatched();

            return match;
        }

        #endregion Static Functions
    }
}
