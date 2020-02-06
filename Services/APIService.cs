using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Team;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace RedditEmblemAPI.Services
{
    public class APIService : IAPIService
    {
        //Attributes
        private SheetsData SheetData;

        public APIService()
        {
            this.SheetData = new SheetsData();
        }

        public SheetsData LoadData(string teamName)
        {
            //Do a deep search for our team file
            string filePath = "";
            foreach (string path in Directory.EnumerateFiles("JSON", "", SearchOption.AllDirectories))
                if (Path.GetFileNameWithoutExtension(path) == teamName)
                {
                    filePath = path;
                    break;
                }

            if (string.IsNullOrEmpty(filePath)) throw new TeamConfigurationNotFoundException(teamName);
            JSONConfiguration config = LoadTeamJSONConfiguration(filePath);

            string mapImageURL, chapterPostURL;
            IList<IList<object>> tileData, terrainTypeData, unitData, itemData, skillData;
            QueryGoogleSheets(config, out mapImageURL, out chapterPostURL, out tileData, out terrainTypeData, out unitData, out itemData, out skillData);

            //Process data
            this.SheetData.Map = new Map(mapImageURL, chapterPostURL, config.Team.Map.Constants, tileData);
            IList<Item> items = ItemsHelper.Process(itemData, config.System.Items);
            IList<Skill> skills = SkillHelper.Process(skillData, config.System.Skills);
            this.SheetData.Units = UnitsHelper.Process(unitData, config.Units, items, skills);

            return this.SheetData;
        }

        public List<string> LoadTeamList()
        {
            List<string> teams = new List<string>();

            //Top directory enumeration
            foreach (string filePath in Directory.EnumerateFiles("JSON"))
            {
                JSONConfiguration config = LoadTeamJSONConfiguration(filePath);
                teams.Add(config.Team.Name);
            }

            return teams;
        }

        private JSONConfiguration LoadTeamJSONConfiguration(string filePath)
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
    
        private void QueryGoogleSheets( JSONConfiguration config,
                                        out string mapImageURL, out string chapterPostURL, 
                                        out IList<IList<object>> tileData,
                                        out IList<IList<object>> terrainTypeData,
                                        out IList<IList<object>> unitData,
                                        out IList<IList<object>> itemData,
                                        out IList<IList<object>> skillData
                                      )
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Execute queries
            ExecuteMapQuery(service, config.Team.WorkbookID, config.Team.Map, out mapImageURL, out chapterPostURL);
            tileData = ExecuteQuery(service, config.Team.WorkbookID, config.Team.Map.Tiles.WorksheetQuery);
            terrainTypeData = ExecuteQuery(service, config.Team.WorkbookID, config.System.TerrainTypes.WorksheetQuery);

            unitData = ExecuteQuery(service, config.Team.WorkbookID, config.Units.WorksheetQuery);
            itemData = ExecuteQuery(service, config.Team.WorkbookID, config.System.Items.WorksheetQuery);
            skillData = ExecuteQuery(service, config.Team.WorkbookID, config.System.Skills.WorksheetQuery);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="workbookID"></param>
        /// <param name="config"></param>
        /// <param name="mapImageURL"></param>
        /// <param name="chapterPostURL"></param>
        /// <exception cref="GoogleSheetsQueryFailedException"></exception>
        private void ExecuteMapQuery(SheetsService service, string workbookID, MapConfig config, out string mapImageURL, out string chapterPostURL)
        {
            try
            {
                GetRequest request = service.Spreadsheets.Values.Get(workbookID, config.WorksheetQuery.ToString());
                request.MajorDimension = config.WorksheetQuery.Orientation;

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
                    throw new MapImageURLNotFoundException(config.WorksheetQuery.Sheet);
                chapterPostURL = (values.ElementAtOrDefault(config.ChapterPostURL) ?? string.Empty).ToString();
            }
            catch(Exception ex) when (ex is MapDataLockedException || ex is MapImageURLNotFoundException)
            {
                //Don't wrap these exception types
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(config.WorksheetQuery.Sheet, ex);
            }
        }

        /// <summary>
        /// Using the specified WorksheetQuery object, calls the Google Sheets API and returns the result. 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="workbookID"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="GoogleSheetsQueryFailedException"></exception>
        private IList<IList<object>> ExecuteQuery(SheetsService service, string workbookID, WorksheetQuery query)
        {
            try
            {
                GetRequest request = service.Spreadsheets.Values.Get(workbookID, query.ToString());
                request.MajorDimension = query.Orientation;

                ValueRange response = request.Execute();
                if (response.Values == null)
                    throw new GoogleSheetsQueryReturnedNullException();
                return response.Values;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(query.Sheet, ex);
            } 
        }
    }
}
