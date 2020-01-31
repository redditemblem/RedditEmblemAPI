using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration;
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
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName + ".json");

            IList<IList<object>> itemData, unitData;
            QueryGoogleSheets(config, out itemData, out unitData);

            //Process data
            IList<Item> items = ItemsHelper.Process(itemData, config.Items);
            this.SheetData.Units = UnitsHelper.Process(unitData, config.Units, items);

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
    
        private void QueryGoogleSheets(JSONConfiguration config, out IList<IList<object>> itemData, out IList<IList<object>> unitData)
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Define query parameters
            // Items
            GetRequest itemsRequest = service.Spreadsheets.Values.Get(config.Team.WorkbookID, config.Items.WorksheetQuery.ToString());
            itemsRequest.MajorDimension = config.Items.WorksheetQuery.Orientation;

            // Units
            GetRequest unitsRequest = service.Spreadsheets.Values.Get(config.Team.WorkbookID, config.Units.WorksheetQuery.ToString());
            unitsRequest.MajorDimension = config.Units.WorksheetQuery.Orientation;

            try
            {
                //Items
                ValueRange itemsResponse = itemsRequest.Execute();
                itemData = itemsResponse.Values;

                //Units
                ValueRange unitResponse = unitsRequest.Execute();
                unitData = unitResponse.Values;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(ex);
            }
        }
        
    }
}
