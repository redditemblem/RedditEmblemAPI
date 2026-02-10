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
        /// Returns a <c>List</c> containing all the <c>IQuery</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<IQuery> GetMapBatchQueries()
        {
            List<IQuery> queries = new List<IQuery>();

            //Essential queries
            queries.AddQueryable(this.Map.MapControls);
            queries.AddQueryable(this.Map.MapTiles);
            queries.AddQueryable(this.System.TerrainTypes);
            queries.AddQueryable(this.Units);
            queries.AddQueryable(this.System.Affiliations);
            queries.AddQueryable(this.System.Items);

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
        /// Returns a <c>List</c> containing all the <c>IQuery</c> objects to be batch queried.
        /// </summary>
        public List<IQuery> GetMapAnalysisBatchQueries()
        {
            List<IQuery> queries = new List<IQuery>();

            //Essential queries
            queries.AddQueryable(this.Map.MapControls);
            queries.AddQueryable(this.Map.MapTiles);
            queries.AddQueryable(this.System.TerrainTypes);
            queries.AddQueryable(this.System.Affiliations);

            return queries;
        }

        /// <summary>
        /// Returns a <c>List</c> containing all the <c>IQuery</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<IQuery> GetConvoyBatchQueries()
        {
            List<IQuery> queries = new List<IQuery>();

            //Essential queries
            queries.AddQueryable(this.System.Items);
            queries.AddQueryable(this.Convoy);

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
        /// Returns a <c>List</c> containing all the <c>IQuery</c> objects to be batch queried.
        /// </summary>
        /// <returns></returns>
        public List<IQuery> GetShopBatchQueries()
        {
            List<IQuery> queries = new List<IQuery>();

            //Essential queries
            queries.AddQueryable(this.System.Items);
            queries.AddQueryable(this.Shop);

            //Add optional queries
            if (this.System.Items.EquippedSkills.Any())
                queries.AddQueryable(this.System.Skills);

            if (this.System.Items.Engravings.Any())
                queries.AddQueryable(this.System.Engravings);

            if (this.System.Items.Tags.Any())
                queries.AddQueryable(this.System.Tags);

            return queries;
        }

        #endregion Batch Query Requests
    }
}