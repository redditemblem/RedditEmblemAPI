using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Convoy;
using RedditEmblemAPI.Models.Output.Shop;
using RedditEmblemAPI.Models.Output.Teams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest;

namespace RedditEmblemAPI.Services
{
    public class APIService : IAPIService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public APIService() { }

        public MapData LoadMapData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);

            string mapImageURL, chapterPostURL;
            QueryGoogleSheets(config, out mapImageURL, out chapterPostURL);

            return new MapData(config, mapImageURL, chapterPostURL);
        }

        public ConvoyData LoadConvoyData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if(config.Convoy == null)
                throw new ConvoyNotConfiguredException();

            QueryGoogleSheets_ConvoyData(config);

            return new ConvoyData(config);
        }

        public ShopData LoadShopData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if(config.Shop == null)
                throw new ShopNotConfiguredException();

            QueryGoogleSheets_ShopData(config);

            return new ShopData(config);
        }

        public IList<TeamData> LoadTeamList()
        {
            IList<TeamData> teams = new List<TeamData>();

            //Top directory enumeration
            foreach (string filePath in Directory.EnumerateFiles("JSON/TeamConfigs/Active"))
            {
                JSONConfiguration config = ReadJSONConfiguration(filePath);
                teams.Add(new TeamData(config.Team.Name, (config.Convoy != null), (config.Shop != null)));
            }

            return teams.OrderBy(t => t.TeamName).ToList();
        }

        private JSONConfiguration LoadTeamJSONConfiguration(string teamName)
        {
            //Do a deep search for our team file, we want to include things in the Hidden folder
            string filePath = "";
            foreach (string path in Directory.EnumerateFiles("JSON/TeamConfigs/Active", "", SearchOption.AllDirectories))
                if (Path.GetFileNameWithoutExtension(path) == teamName)
                {
                    filePath = path;
                    break;
                }

            if (string.IsNullOrEmpty(filePath)) throw new TeamConfigurationNotFoundException(teamName);
            return ReadJSONConfiguration(filePath);
        }

        private JSONConfiguration ReadJSONConfiguration(string filePath)
        {
            try
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (JSONConfiguration)serializer.Deserialize(file, typeof(JSONConfiguration));
                }
            }
            catch(FileNotFoundException ex)
            {
                throw new TeamConfigurationNotFoundException(Path.GetFileNameWithoutExtension(filePath));
            }
        }

        #region Google Sheet Queries

        private void QueryGoogleSheets( JSONConfiguration config, out string mapImageURL, out string chapterPostURL)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Execute queries
            ExecuteMapQuery(service, config.Team.WorkbookID, config.Team.Map, out mapImageURL, out chapterPostURL);

            IList<Query> queries = config.GetMapBatchQueries();

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.ROWS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.ROWS).ToList()
                             );

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.COLUMNS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.COLUMNS).ToList()
                             );
        }

        private void QueryGoogleSheets_ConvoyData(JSONConfiguration config)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            IList<Query> queries = config.GetConvoyBatchQueries();

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.ROWS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.ROWS).ToList()
                             );

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.COLUMNS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.COLUMNS).ToList()
                             );
        }

        private void QueryGoogleSheets_ShopData(JSONConfiguration config)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            IList<Query> queries = config.GetShopBatchQueries();

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.ROWS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.ROWS).ToList()
                             );

            ExecuteBatchQuery(service,
                              config.Team.WorkbookID,
                              MajorDimensionEnum.COLUMNS,
                              queries.Where(q => q != null && q.Orientation == MajorDimensionEnum.COLUMNS).ToList()
                             );
        }

        /// <summary>
        /// Executes queries to check the map status (on/off) and retrieve the map image URL and chapter post URL.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="workbookID"></param>
        /// <param name="config"></param>
        /// <param name="mapImageURL"></param>
        /// <param name="chapterPostURL"></param>
        /// <exception cref="MapDataLockedException"></exception>
        /// <exception cref="MapImageURLNotFoundException"></exception>
        /// <exception cref="GoogleSheetsQueryFailedException"></exception>
        private void ExecuteMapQuery(SheetsService service, string workbookID, MapConfig config, out string mapImageURL, out string chapterPostURL)
        {
            try
            {
                GetRequest request = service.Spreadsheets.Values.Get(workbookID, config.Query.ToString());
                request.MajorDimension = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;// config.Query.Orientation;

                ValueRange response = request.Execute();
                if (response.Values == null)
                    throw new GoogleSheetsQueryReturnedNullException();

                IList<object> values = response.Values.First();

                //Check to make sure the map is not locked
                if ((values.ElementAtOrDefault(config.MapSwitch) ?? "Off").ToString() != "On")
                    throw new MapDataLockedException();

                //Return URLs
                mapImageURL = (values.ElementAtOrDefault(config.MapURL) ?? string.Empty).ToString();
                if (string.IsNullOrEmpty(mapImageURL))
                    throw new MapImageURLNotFoundException(config.Query.Sheet);
                chapterPostURL = (values.ElementAtOrDefault(config.ChapterPostURL) ?? string.Empty).ToString();
            }
            catch(Exception ex) when (ex is MapDataLockedException || ex is MapImageURLNotFoundException)
            {
                //Don't wrap these exception types
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(config.Query.Sheet, ex);
            }
        }

        /// <summary>
        /// Using the set of specified Query objects, executes a batch query on the Google Sheets API and sets their Data values. 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="workbookID"></param>
        /// <param name="queries">The set of queries to be executed. All are expected to have the same <c>Orientation</c> value as <paramref name="dimension"/>.</param>
        /// <returns></returns>
        /// <exception cref="GoogleSheetsQueryFailedException"></exception>
        private void ExecuteBatchQuery(SheetsService service, string workbookID, MajorDimensionEnum dimension, IList<Query> queries)
        {
            try
            {
                if (queries.Count == 0)
                    return;

                BatchGetRequest request = service.Spreadsheets.Values.BatchGet(workbookID);
                request.Ranges = queries.Select(q => q.ToString()).ToList();
                request.MajorDimension = dimension;

                BatchGetValuesResponse response = request.Execute();

                int i = 0;
                foreach (Query query in queries)
                {
                    if (response.ValueRanges.ElementAtOrDefault(i).Values == null)
                        throw new GoogleSheetsQueryReturnedNullException();
                    query.Data = response.ValueRanges.ElementAtOrDefault(i).Values;

                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(string.Join(", ", queries.Select(q => q.Sheet)), ex);
            } 
        }

        #endregion
    }
}
