using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class CalculatedStatEquationConfig
    {
        #region Required Fields

        /// <summary>
        /// Dynamically-executed equation to calculate the stat's value.
        /// </summary>
        [JsonRequired]
        public string Equation { get; set; }

        #endregion Required Fields

        #region Optional Fields

        /// <summary>
        /// Optional. The selection value associated with this equation.
        /// </summary>
        public string SelectValue { get; set; } = string.Empty;

        #endregion
    }
}
