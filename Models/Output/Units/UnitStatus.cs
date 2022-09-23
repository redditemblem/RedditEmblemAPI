using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
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
        #warning "Team-specific temporary code. Uncomment ignore ASAP."
        //[JsonIgnore]
        public string FullName { get; set; }

        /// <summary>
        /// TEMPORARY - HEALER EMBLEM
        /// Flag indicating that we should use the full status name instead of the pretty parsed one
        /// </summary>
        #warning "Team-specific temporary code. Remove ASAP."
        public bool UseFullName { get; set; }

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

        #endregion

        private static Regex turnsRegex = new Regex(@"\([0-9]+\)"); //match status turns (ex. "(5)")
        private static Regex bracketsRegex = new Regex(@"\[.+\]"); //match *anything* surrounded by square brackets

        /// <summary>
        /// Searches for a <c>StatusCondition</c> in <paramref name="statusConditions"/> that matches <paramref name="fullStatusName"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatusConditionException"></exception>
        public UnitStatus(string fullStatusName, IDictionary<string, StatusCondition> statusConditions)
        {
            this.FullName = fullStatusName;
            this.RemainingTurns = 0;

            string name = this.FullName;

            //Search for turns syntax
            Match turnsMatch = turnsRegex.Match(name);
            if (turnsMatch.Success)
            {
                string t = turnsMatch.Value.ToString();
                t = t.Substring(1, t.Length - 2);
                this.RemainingTurns = int.Parse(t);
                name = turnsRegex.Replace(name, string.Empty);
            }

            //TEMPORARY - HEALER EMBLEM
            //Remove anything between square brackets
            #warning "Team-specific temporary code. Remove ASAP."
            Match bracketMatch = bracketsRegex.Match(name);
            if(bracketMatch.Success)
            {
                name = bracketsRegex.Replace(name, string.Empty);
                this.UseFullName = true;
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
