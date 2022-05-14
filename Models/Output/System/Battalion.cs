using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing a battalion definition in the team's system. 
    /// </summary>
    public class Battalion
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

        public Battalion(BattalionsConfig config, List<string> data, IDictionary<string, Gambit> gambits)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.GambitObj = MatchGambit(config, data, gambits);
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxEndurance = DataParser.Int_Positive(data, config.MaxEndurance, "Max Endurance");
            this.Rank = DataParser.OptionalString(data, config.Rank, "Rank");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            BuildStats(config.Stats, data);
            BuildStatModifiers(config.StatModifiers, data);
        }

        private Gambit MatchGambit(BattalionsConfig config, List<string> data, IDictionary<string, Gambit> gambits)
        {
            string name = DataParser.String(data, config.Gambit, "Gambit");

            Gambit gambit;
            if (!gambits.TryGetValue(name, out gambit))
                throw new UnmatchedGambitException(name);

            return gambit;
        }

        private void BuildStats(List<NamedStatConfig> configs, List<string> data)
        {
            this.Stats = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName);
                this.Stats.Add(stat.SourceName, val);
            }
        }

        private void BuildStatModifiers(List<NamedStatConfig> configs, List<string> data)
        {
            this.StatModifiers = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, $"{stat.SourceName} Modifier");
                if (val == 0) continue;
                this.StatModifiers.Add(stat.SourceName, val);
            }
        }

        #region Static Functions

        public static IDictionary<string, Battalion> BuildDictionary(BattalionsConfig config, IDictionary<string, Gambit> gambits)
        {
            IDictionary<string, Battalion> battalions = new Dictionary<string, Battalion>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> battalion = row.Select(r => r.ToString()).ToList();
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

        #endregion

    }
}
