using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
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

        #endregion

        private static Regex turnsRegex = new Regex(@"\([0-9]+\)"); //match status turns (ex. "(5)")

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

            name = name.Trim();

            StatusCondition match;
            if (!statusConditions.TryGetValue(name, out match))
                throw new UnmatchedStatusConditionException(name);
            this.StatusObj = match;
            match.Matched = true;
        }
    }
}
