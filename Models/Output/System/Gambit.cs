using Newtonsoft.Json;
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
    /// Object representing a gambit definition in the team's system. 
    /// </summary>
    public class Gambit : IMatchable
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this gambit was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; private set; }

        /// <summary>
        /// The gambit's name. 
        /// </summary>
        public string Name { get; set; }

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
        public GambitRange Range { get; set; }

        /// <summary>
        /// Collection of stat values for the gambit. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Any text information about the gambit to display.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        public Gambit(GambitsConfig config, IEnumerable<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxUses = DataParser.Int_Positive(data, config.MaxUses, "Max Uses");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Range = new GambitRange(config.Range, data);
            this.Stats = DataParser.NamedStatDictionary_Int_Any(config.Stats, data, true);
        }

        /// <summary>
        /// Sets the <c>Matched</c> flag for this <c>Gambit</c> to true. Additionally, calls <c>FlagAsMatched()</c> for all of its <c>IMatchable</c> child attributes.
        /// </summary>
        public void FlagAsMatched()
        {
            this.Matched = true;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Gambit</c> from each valid row.
        /// </summary>
        /// <exception cref="GambitProcessingException"></exception>
        public static IDictionary<string, Gambit> BuildDictionary(GambitsConfig config)
        {
            IDictionary<string, Gambit> gambits = new Dictionary<string, Gambit>();
            if (config == null || config.Queries == null)
                return gambits;

            foreach (List<object> row in config.Queries.SelectMany(q => q.Data))
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
        /// Matches each of the strings in <paramref name="names"/> to a <c>Gambit</c> in <paramref name="gambits"/> and returns the matches as a list.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned objects to true.</param>
        public static List<Gambit> MatchNames(IDictionary<string, Gambit> gambits, IEnumerable<string> names, bool skipMatchedStatusSet = false)
        {
            return names.Select(n => MatchName(gambits, n, skipMatchedStatusSet)).ToList();
        }

        /// <summary>
        /// Matches <paramref name="name"/> to a <c>Gambit</c> in <paramref name="gambits"/> and returns it.
        /// </summary>
        /// <param name="skipMatchedStatusSet">If true, will not set the <c>Matched</c> flag on the returned object to true.</param>
        /// <exception cref="UnmatchedGambitException"></exception>
        public static Gambit MatchName(IDictionary<string, Gambit> gambits, string name, bool skipMatchedStatusSet = false)
        {
            Gambit match;
            if (!gambits.TryGetValue(name, out match))
                throw new UnmatchedGambitException(name);
            
            if(!skipMatchedStatusSet) match.FlagAsMatched();

            return match;
        }

        #endregion

    }
}
