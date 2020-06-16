using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Configuration.Map;
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
        /// Required. Container object for team configuration.
        /// </summary>
        [JsonRequired]
        public TeamConfig Team { get; set; }

        /// <summary>
        /// Required. Container object for map configuration.
        /// </summary>
        [JsonRequired]
        public MapConfig Map { get; set; }

        /// <summary>
        /// Required. Container object for system configuration.
        /// </summary>
        [JsonRequired]
        public SystemConfig System { get; set; }

        /// <summary>
        /// Required. Container object for units configuration.
        /// </summary>
        [JsonRequired]
        public UnitsConfig Units { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Container object for convoy configuration.
        /// </summary>
        public ConvoyConfig Convoy { get; set; } = null;

        /// <summary>
        /// Optional. Container object for shop configuration.
        /// </summary>
        public ShopConfig Shop { get; set; } = null;

        #endregion

        #region Batch Query Requests

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetMapBatchQueries()
        {
            //Essential queries
            IList<Query> queries = new List<Query>()
            {
                this.Map.MapControls.Query,
                this.Map.MapTiles.Query,
                this.System.TerrainTypes.Query,
                this.Units.Query,
                this.System.Classes.Query,
                this.System.Affiliations.Query,
                this.System.Items.Query
            };

            //Add optional queries
            if (this.Map.MapEffects != null) queries.Add(this.Map.MapEffects.Query);
            if (this.System.TerrainEffects != null) queries.Add(this.System.TerrainEffects.Query);
            if (this.System.Skills != null) queries.Add(this.System.Skills.Query);
            if (this.System.StatusConditions != null) queries.Add(this.System.StatusConditions.Query);

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public IList<Query> GetConvoyBatchQueries()
        {
            //Essential queries
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
            //Essential queries
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