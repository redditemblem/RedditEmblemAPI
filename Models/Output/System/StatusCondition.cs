using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.System.Statuses;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    public class StatusCondition
    {
        /// <summary>
        /// Flag indicating whether or not this status was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The name of the status condition.
        /// </summary>
        public string Name { get; set; }

        public StatusType Type { get; set; }

        public int Turns { get; set; }

        public IList<string> TextFields { get; set; }

        public StatusCondition(StatusConditionConfig config, IList<string> data)
        {
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.Type = ParseStatusConditionType(data, config.Type);
            this.Turns = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.Turns), "Turns", true, 0);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
        }

        private StatusType ParseStatusConditionType(IList<string> data, int index)
        {
            string name = data.ElementAtOrDefault<string>(index).Trim();
            switch (name)
            {
                case "Positive": return StatusType.Positive;
                case "Negative": return StatusType.Negative;
                case "Neutral": return StatusType.Neutral;
                default: throw new Exception(string.Format("Status condition type \"{0}\" not recognized.", name));
            }
        }
    }

    public enum StatusType
    {
        Positive = 0,
        Negative = 1,
        Neutral = 2
    }
}
