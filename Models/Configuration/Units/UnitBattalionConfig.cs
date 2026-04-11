using Newtonsoft.Json;

namespace RedditEmblemAPI.Models.Configuration.Units
{
    public class UnitBattalionConfig
    {
        #region Required Fields

        /// <summary>
        /// Required. Location of the name of the unit's battalion.
        /// </summary>
        [JsonRequired]
        public (int, int) Battalion { get; set; }

        /// <summary>
        /// Required. Location of the remaining endurance for the unit's battalion.
        /// </summary>
        [JsonRequired]
        public (int, int) Endurance { get; set; }

        /// <summary>
        /// Required. Location of the remaining number of uses for the unit's battalion's gambit.
        /// </summary>
        [JsonRequired]
        public (int, int) GambitUses { get; set; }

        #endregion Required Fields
    }
}
