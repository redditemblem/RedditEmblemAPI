using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace RedditEmblemAPI.Services
{
    public class APIService : IAPIService
    {
        public SheetsData SheetData;

        public APIService()
        {
            this.SheetData = new SheetsData();
        }

        public SheetsData LoadData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName + ".json");
            QueryGoogleSheets(config);

            return this.SheetData;
        }

        public List<string> LoadTeamList()
        {
            List<string> teams = new List<string>();

            foreach (string fileName in Directory.EnumerateFiles("JSON").Select(Path.GetFileName))
            {
                JSONConfiguration config = LoadTeamJSONConfiguration(fileName);
                teams.Add(config.Team.Name);
            }

            return teams;
        }

        private JSONConfiguration LoadTeamJSONConfiguration(string fileName)
        {
            try
            {
                using (StreamReader file = File.OpenText(@"JSON\" + fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (JSONConfiguration)serializer.Deserialize(file, typeof(JSONConfiguration));
                }
            }
            catch(FileNotFoundException ex)
            {
                throw new TeamConfigurationNotFoundException(fileName);
            }
        }
    
        private IList<IList<object>> QueryGoogleSheets(JSONConfiguration config)
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Define request parameters.
            GetRequest request =
                    service.Spreadsheets.Values.Get(config.Team.WorkbookID, config.Units.WorksheetQuery.ToString());
            request.MajorDimension = GetRequest.MajorDimensionEnum.COLUMNS; //config.Units.WorksheetQuery.Orientation;

            try
            {
                ValueRange data = request.Execute();
                return data.Values;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsAccessDeniedException();
            }
            
        }
    }
}
