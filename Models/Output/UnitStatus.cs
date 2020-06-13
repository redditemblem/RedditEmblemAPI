using Newtonsoft.Json;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output
{
    public class UnitStatus
    {
        /// <summary>
        /// The full name of the status condition pulled from raw Unit data.
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

        private static Regex turnsRegex = new Regex(@"\([0-9]+\)"); //match status turns (ex. "(5)")

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitStatus(string fullName, IDictionary<string, StatusCondition> statuses)
        {
            this.FullName = fullName;
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
            if (!statuses.TryGetValue(name, out match))
                throw new UnmatchedStatusException(name);

            this.StatusObj = match;
            match.Matched = true;
        }
    }
}
