using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Input.Turns;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Storage.Convoy;
using RedditEmblemAPI.Models.Output.Storage.Shop;
using RedditEmblemAPI.Models.Output.Teams;
using RedditEmblemAPI.Models.Output.Turns;
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

        /// <summary>
        /// Returns JSON data for displaying a team's map.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public MapData LoadMapData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            QueryGoogleSheets(config, config.GetMapBatchQueries());

            return new MapData(config);
        }

        /// <summary>
        /// Returns JSON data for the turns submitted for a team's map.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public TurnData LoadMapTurnData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if (config.Turns == null)
                throw new TurnsNotConfiguredException();
            QueryGoogleSheets(config, config.GetMapTurnsBatchQueries());

            return new TurnData(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="teamName"></param>
        public void CreateTeamMapTurn(string teamName, ClientTurnData postData)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if (config.Turns == null)
                throw new TurnsNotConfiguredException();

            QueryGoogleSheets(config, config.GetMapTurnsBatchQueries());
            TurnData existingData = new TurnData(config);

            //Set the new turn's turn ID
            postData.NewTurn.TurnID = existingData.SubmittedTurns.Select(t => t.TurnID).Max() + 1;

            //Check turn order validity
            int maxTurnOrder = existingData.SubmittedTurns.Select(t => t.TurnOrder).Max();
            if (postData.NewTurn.TurnOrder > maxTurnOrder + 1)
                throw new Exception("Invalid turn order");

            if(postData.NewTurn.TurnOrder == maxTurnOrder + 1)
            {
                //If we're inserting a turn at the end of the list, just add it
                PostGoogleSheets_CreateTurn(config, postData.NewTurn);
            }
        }

        /// <summary>
        /// Returns JSON data for displaying a team's convoy.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public ConvoyData LoadConvoyData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if(config.Convoy == null)
                throw new ConvoyNotConfiguredException();
            QueryGoogleSheets(config, config.GetConvoyBatchQueries());

            return new ConvoyData(config);
        }

        /// <summary>
        /// Returns JSON data for displaying a team's shop.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public ShopData LoadShopData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName);
            if(config.Shop == null)
                throw new ShopNotConfiguredException();
            QueryGoogleSheets(config, config.GetShopBatchQueries());

            return new ShopData(config);
        }

        /// <summary>
        /// Returns an alphabetical list of team configurations present within the "JSON/TeamConfigs/Active" file directory.
        /// </summary>
        /// <returns></returns>
        public IList<TeamData> LoadTeamList()
        {
            IList<TeamData> teams = new List<TeamData>();

            //Top directory enumeration
            foreach (string filePath in Directory.EnumerateFiles("JSON/TeamConfigs/Active"))
            {
                JSONConfiguration config = DeserializeJSONConfiguration(filePath);
                teams.Add(new TeamData(config.Team.Name, (config.Convoy != null), (config.Shop != null)));
            }

            return teams.OrderBy(t => t.TeamName).ToList();
        }

        #region JSON File Processing

        /// <summary>
        /// Performs a deep search on the "JSON/TeamConfigs/Active" directory for a file name that matches <paramref name="teamName"/>. If found, deserializes and returns the <c>JSONConfiguration</c> data contained within.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        /// <exception cref="TeamConfigurationNotFoundException"></exception>
        private JSONConfiguration LoadTeamJSONConfiguration(string teamName)
        {
            //Do a deep search for our team file, we want to include things in the Hidden folder
            string filePath = "";
            foreach (string path in Directory.EnumerateFiles("JSON/TeamConfigs/Active", "", SearchOption.AllDirectories))
                if (string.Equals(Path.GetFileNameWithoutExtension(path), teamName, StringComparison.OrdinalIgnoreCase))
                {
                    filePath = path;
                    break;
                }

            if (string.IsNullOrEmpty(filePath)) throw new TeamConfigurationNotFoundException(teamName);
            return DeserializeJSONConfiguration(filePath);
        }

        /// <summary>
        /// Opens the file located at <paramref name="filePath"/> and deserializes its contents into a <c>JSONConfiguration</c> object.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="TeamConfigurationNotFoundException"></exception>
        private JSONConfiguration DeserializeJSONConfiguration(string filePath)
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

        #endregion

        #region Google Sheet Queries

        /// <summary>
        /// Divides the list of <c>Query</c> objects into horizontal and vertical dimension subsets, then executes a batch query on each.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="queries"></param>
        private void QueryGoogleSheets( JSONConfiguration config, IList<Query> queries)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

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
        /// Using the set of <c>Query</c> objects, executes a batch query on the Google Sheets API and stores the returned values in the <c>Query</c>'s Data attributes.
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
                    {
                        if (query.AllowNullData) query.Data = new List<IList<object>>();
                        else throw new GoogleSheetsQueryReturnedNullException(query.Sheet);
                    }
                    else
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

        #region Google Sheet Posts

        private void PostGoogleSheets_CreateTurn(JSONConfiguration config, Turn newTurn)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            ValueRange valueRange = new ValueRange();
            valueRange.Values = newTurn.ToDataMatrix();

            AppendRequest request = service.Spreadsheets.Values.Append(valueRange, config.Team.WorkbookID, string.Empty);
            request.InsertDataOption = AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request.ValueInputOption = AppendRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();
        }

        #endregion
    }
}
