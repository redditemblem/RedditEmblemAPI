using RedditEmblemAPI.Models.Configuration.System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class System
    {
        public System()
        {
            this.Classes = new Dictionary<string, Class>();
            this.TerrainTypes = new Dictionary<string, TerrainType>();
        }

        /// <summary>
        /// Container dictionary for data about classes.
        /// </summary>
        public IDictionary<string, Class> Classes { get; set; }

        /// <summary>
        /// Container dictionary for data about terrain types.
        /// </summary>
        public IDictionary<string, TerrainType> TerrainTypes { get; set; }

        /// <summary>
        /// Container for currency constants.
        /// </summary>
        public CurrencyConstsConfig Currency { get; set; }
    }
}
