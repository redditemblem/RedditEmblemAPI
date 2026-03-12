using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitStatus"/>
    public interface IUnitStatus
    {
        /// <inheritdoc cref="UnitStatus.FullName"/>
        string FullName { get; }

        /// <inheritdoc cref="UnitStatus.Status"/>
        IStatusCondition Status { get; }

        /// <inheritdoc cref="UnitStatus.RemainingTurns"/>
        int RemainingTurns { get; }

        /// <inheritdoc cref="UnitStatus.AdditionalStats"/>
        IDictionary<string, int> AdditionalStats { get; }
    }

    #endregion Interface

    /// <summary>
    /// Object representing a <c>StatusCondition</c> present on a <c>Unit</c>.
    /// </summary>
    public class UnitStatus : IUnitStatus
    {
        #region Attributes

        /// <summary>
        /// The full name of the status condition pulled from raw <c>Unit</c> data.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; private set; }

        /// <summary>
        /// The <c>IStatusCondition</c> object.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(MatchableNameConverter))]
        public IStatusCondition Status { get; private set; }

        /// <summary>
        /// The number of turns this status has left.
        /// </summary>
        public int RemainingTurns { get; private set; }

        /// <summary>
        /// Dictionary of additional stat values for this status.
        /// </summary>
        public IDictionary<string, int> AdditionalStats { get; private set; }

        #endregion Attributes

        private static Regex turnsRegex = new Regex(@"\([0-9]+\)"); //match status turns (ex. "(5)")

        /// <summary>
        /// Searches for a <c>StatusCondition</c> in <paramref name="statusConditions"/> that matches <paramref name="fullStatusName"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatusConditionException"></exception>
        public UnitStatus(IEnumerable<string> data, UnitStatusConditionConfig config, IDictionary<string, IStatusCondition> statusConditions)
        {
            this.FullName = DataParser.String(data, config.Name, "Status Condition Name");
            this.RemainingTurns = 0;

            string name = this.FullName;

            if (config.RemainingTurns != -1)
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
            foreach (NamedStatConfig stat in config.AdditionalStats)
            {
                int value = DataParser.OptionalInt_Any(data, stat.Value, $"{this.FullName} {stat.SourceName}");
                if (value == 0) continue;

                this.AdditionalStats.Add(stat.SourceName, value);
            }

            name = name.Trim();
            this.Status = StatusCondition.MatchName(statusConditions, name);
        }

        #region Static Functions

        /// <summary>
        /// Builds and returns a list of the unit's status conditions.
        /// </summary>
        public static List<IUnitStatus> BuildList(IEnumerable<string> data, List<UnitStatusConditionConfig> configs, IDictionary<string, IStatusCondition> statuses)
        {
            List<IUnitStatus> statusConditions = new List<IUnitStatus>();

            foreach (UnitStatusConditionConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Status Condition Name");
                if (string.IsNullOrEmpty(name)) continue;

                statusConditions.Add(new UnitStatus(data, config, statuses));
            }

            return statusConditions;
        }

        #endregion Static Functions
    }
}
