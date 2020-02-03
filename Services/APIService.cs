using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Configuration.Common;
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

            IList<IList<object>> unitData, itemData, skillData;
            QueryGoogleSheets(config, out unitData, out itemData, out skillData);

            //Process data
            IList<Item> items = ItemsHelper.Process(itemData, config.Items);
            IList<Skill> skills = SkillHelper.Process(skillData, config.Skills);
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
    
        private void QueryGoogleSheets(JSONConfiguration config, out IList<IList<object>> unitData, out IList<IList<object>> itemData, out IList<IList<object>> skillData)
        {
            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Execute queries
            unitData = ExecuteQuery(service, config.Team.WorkbookID, config.Units.WorksheetQuery);
            itemData = ExecuteQuery(service, config.Team.WorkbookID, config.Items.WorksheetQuery);
            skillData = ExecuteQuery(service, config.Team.WorkbookID, config.Skills.WorksheetQuery);
        }

        private IList<IList<object>> ExecuteQuery(SheetsService service, string workbookID, WorksheetQuery query)
        {
            try
            {
                GetRequest request = service.Spreadsheets.Values.Get(workbookID, query.ToString());
                request.MajorDimension = query.Orientation;

                ValueRange response = request.Execute();
                return response.Values;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(query.Sheet, ex);
            } 
        }
    }
}
