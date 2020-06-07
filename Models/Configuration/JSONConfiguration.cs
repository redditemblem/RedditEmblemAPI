using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Configuration.Units;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration
{
    /// <summary>
    /// Container class for deserialized JSON configuration data.
    /// </summary>
    public class JSONConfiguration
    {
        #region RequiredFields

        /// <summary>
        /// Container object for team configuration.
        /// </summary>
        [JsonRequired]
        public TeamConfig Team { get; set; }

        /// <summary>
        /// Container object for system configuration.
        /// </summary>
        [JsonRequired]
        public SystemConfig System { get; set; }

        /// <summary>
        /// Container object for units configuration.
        /// </summary>
        [JsonRequired]
        public UnitsConfig Units { get; set; }

        #endregion

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetBatchQueries()
        {
            IList<Query> queries = new List<Query>()
            {
                this.Team.Map.Tiles.Query,
                this.System.TerrainTypes.Query,
                this.Units.Query,
                this.System.Items.Query,
                this.System.Skills.Query,
                this.System.Classes.Query,
                this.System.Affiliations.Query,
                this.System.TerrainTypes.Query
            };

            //Add optional items
            if (this.System.Statuses != null)
                queries.Add(this.System.Statuses.Query);

            return queries;
        }
    }
}