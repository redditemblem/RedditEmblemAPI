using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Configuration.Shop;
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
        #region Required Fields

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

        #region Optional Fields

        /// <summary>
        /// Container object for convoy configuration.
        /// </summary>
        public ConvoyConfig Convoy { get; set; }

        /// <summary>
        /// Container object for shop configuration.
        /// </summary>
        public ShopConfig Shop { get; set; }

        #endregion

        #region Batch Query Requests

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetMapBatchQueries()
        {
            IList<Query> queries = new List<Query>()
            {
                this.Team.Map.Query,
                this.Team.Map.Tiles.Query,
                this.System.TerrainTypes.Query,
                this.Units.Query,
                this.System.Items.Query,
                this.System.Classes.Query,
                this.System.Affiliations.Query
            };

            //Add optional items
            if (this.Team.Map.Effects != null) queries.Add(this.Team.Map.Effects.Query);
            if (this.System.Skills != null) queries.Add(this.System.Skills.Query);
            if (this.System.Statuses != null) queries.Add(this.System.Statuses.Query);
            if (this.System.TerrainEffects != null) queries.Add(this.System.TerrainEffects.Query);

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetConvoyBatchQueries()
        {
            IList<Query> queries = new List<Query>()
            {
                this.System.Items.Query,
                this.Convoy.Query
            };

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetShopBatchQueries()
        {
            IList<Query> queries = new List<Query>()
            {
                this.System.Items.Query,
                this.Shop.Query
            };

            return queries;
        }

        #endregion
    }
}