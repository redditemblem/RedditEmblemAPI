using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a <c>StatusCondition</c> present on a <c>Unit</c>.
    /// </summary>
    public class UnitStatus
    {
        #region Attributes

        /// <summary>
        /// The full name of the status condition pulled from raw <c>Unit</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// Only for JSON serialization. The name of the status condition. 
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.StatusObj.Name; } }

        /// <summary>
        /// The <c>StatusCondition</c> object.
        /// </summary>
        [JsonIgnore]
        public StatusCondition StatusObj { get; set; }

        /// <summary>
        /// The number of turns this status has left.
        /// </summary>
        public int RemainingTurns { get; set; }

        /// <summary>
        /// Dictionary of additional stat values for this status.
        /// </summary>
        public IDictionary<string, int> AdditionalStats { get; set; }

        #endregion

        private static Regex turnsRegex = new Regex(@"\([0-9]+\)"); //match status turns (ex. "(5)")
        private static Regex bracketsRegex = new Regex(@"\[([0-9]+)\]"); //match values surrounded by square brackets

        /// <summary>
        /// Searches for a <c>StatusCondition</c> in <paramref name="statusConditions"/> that matches <paramref name="fullStatusName"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatusConditionException"></exception>
        public UnitStatus(List<string> data, UnitStatusConditionConfig config, IDictionary<string, StatusCondition> statusConditions)
        {
            this.FullName = DataParser.String(data, config.Name, "Status Condition Name");
            this.RemainingTurns = 0;

            string name = this.FullName;

            if(config.RemainingTurns != -1)
            {
                this.RemainingTurns = DataParser.OptionalInt_Positive(data, config.RemainingTurns, $"{this.FullName} Remaining Turns");  
            }
            else
            {
                //Search for turns syntax in status name
                Match turnsMatch = turnsRegex.Match(name);
                if (turnsMatch.Success)
                {
                    string t = turnsMatch.Value.ToString();
                    t = t.Substring(1, t.Length - 2);
                    this.RemainingTurns = int.Parse(t);
                    name = turnsRegex.Replace(name, string.Empty);
                }
            }

            this.AdditionalStats = new Dictionary<string, int>();
            foreach(NamedStatConfig stat in config.AdditionalStats)
            {
                int value = DataParser.OptionalInt_Any(data, stat.Value, $"{this.FullName} {stat.SourceName}");
                if (value == 0) continue;

                this.AdditionalStats.Add(stat.SourceName, value);
            }

            //TEMPORARY - HEALER EMBLEM
            //Remove anything between square brackets
            #warning "Team-specific temporary code. Remove ASAP."
            Match bracketMatch = bracketsRegex.Match(name);
            if (bracketMatch.Success)
            {
                this.AdditionalStats.Add("Potency", int.Parse(bracketMatch.Groups[1].Value));
                name = bracketsRegex.Replace(name, string.Empty);
            }
            //END TEMP

            name = name.Trim();

            StatusCondition match;
            if (!statusConditions.TryGetValue(name, out match))
                throw new UnmatchedStatusConditionException(name);
            this.StatusObj = match;
            match.Matched = true;
        }
    }
}
