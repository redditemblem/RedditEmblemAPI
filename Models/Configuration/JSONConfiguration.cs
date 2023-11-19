using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Convoy;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Configuration.Shop;
using RedditEmblemAPI.Models.Configuration.System;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Configuration.Units;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Configuration
{
    /// <summary>
    /// Container class for deserialized JSON configuration data. The top-level element.
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
        public List<Query> GetMapBatchQueries()
        {
            //Essential queries
            List<Query> queries = new List<Query>()
            {
                this.Map.MapControls.Query,
                this.Map.MapTiles.Query,
                this.System.TerrainTypes.Query,
                this.Units.Query,
                this.System.Affiliations.Query,
                this.System.Items.Query
            };

            //Add optional queries
            queries.AddQueryable(this.Map.MapObjects);
            queries.AddQueryable(this.System.TileObjects);
            queries.AddQueryable(this.System.Classes);
            queries.AddQueryable(this.System.Skills);
            queries.AddQueryable(this.System.StatusConditions);
            queries.AddQueryable(this.System.Tags);
            queries.AddQueryable(this.System.WeaponRankBonuses);
            queries.AddQueryable(this.System.Engravings);
            queries.AddQueryable(this.System.CombatArts);
            queries.AddQueryable(this.System.Battalions);
            queries.AddQueryable(this.System.Gambits);
            queries.AddQueryable(this.System.BattleStyles);
            queries.AddQueryable(this.System.Emblems);
            queries.AddQueryable(this.System.EngageAttacks);
            queries.AddQueryable(this.System.Adjutants);

            return queries;
        }


        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<Query> GetMapAnalysisBatchQueries()
        {
            //Essential queries
            List<Query> queries = new List<Query>()
            {
                this.Map.MapControls.Query,
                this.Map.MapTiles.Query,
                this.System.TerrainTypes.Query,
                this.System.Affiliations.Query
            };

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<Query> GetConvoyBatchQueries()
        {
            //Essential queries
            List<Query> queries = new List<Query>()
            {
                this.System.Items.Query,
                this.Convoy.Query
            };

            //Add optional queries
            if(this.System.Items.EquippedSkills.Any())
                queries.AddQueryable(this.System.Skills);

            if(this.System.Items.Engravings.Any())
                queries.AddQueryable(this.System.Engravings);

            if(this.System.Items.Tags.Any())
                queries.AddQueryable(this.System.Tags);

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>Query</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<Query> GetShopBatchQueries()
        {
            //Essential queries
            List<Query> queries = new List<Query>()
            {
                this.System.Items.Query,
                this.Shop.Query
            };

            //Add optional queries
            if (this.System.Items.EquippedSkills.Any())
                queries.AddQueryable(this.System.Skills);

            if (this.System.Items.Engravings.Any())
                queries.AddQueryable(this.System.Engravings);

            if (this.System.Items.Tags.Any())
                queries.AddQueryable(this.System.Tags);

            return queries;
        }

        #endregion
    }
}