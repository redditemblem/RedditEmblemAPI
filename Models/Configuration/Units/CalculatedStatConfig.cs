using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units.CalculatedStats
{
    public class CalculatedStatConfig
    {
        /// <summary>
        /// Name of the calculated stat. (Ex. Atk)
        /// </summary>
        [JsonRequired]
        public string SourceName { get; set; }

        /// <summary>
        /// Dynamically-executed equation to calculate the stat's value.
        /// </summary>
        [JsonRequired]
        public string Equation { get; set; }
    }
}
