using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using RedditEmblemAPI.Models;
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
        //Constants
        private const int INDEX_UNIT_DATA = 0;

        public SheetsData SheetData;

        public APIService()
        {
            this.SheetData = new SheetsData();
        }

        public SheetsData LoadData(string teamName)
        {
            JSONConfiguration config = LoadTeamJSONConfiguration(teamName + ".json");

            IList<IList<object>> unitData;
            QueryGoogleSheets(config, out unitData);

            //Process data arrays
            ProcessUnits(unitData, config.Units);

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
    
        private void QueryGoogleSheets(JSONConfiguration config, out IList<IList<object>> unitData)
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "RedditEmblemAPI",
                ApiKey = Environment.GetEnvironmentVariable("APIKey")
            });

            // Define query parameters
            // Units
            GetRequest unitsRequest = service.Spreadsheets.Values.Get(config.Team.WorkbookID, config.Units.WorksheetQuery.ToString());
            unitsRequest.MajorDimension = config.Units.WorksheetQuery.Orientation;

            try
            {
                ValueRange unitRepsonse = unitsRequest.Execute();
                unitData = unitRepsonse.Values;
            }
            catch (Exception ex)
            {
                throw new GoogleSheetsQueryFailedException(ex);
            }
        }
        
        private void ProcessUnits(IList<IList<object>> data, UnitsConfig config)
        {
            foreach(IList<object> row in data)
            {
                try
                {
                    //Convert objects to strings
                    IList<string> unit = row.Select(r => r.ToString()).ToList();

                    Unit temp = new Unit()
                    {
                        Name = unit[config.UnitName],
                        SpriteURL = unit[config.SpriteURL],
                        Coordinates = new Coordinate(unit[config.Coordinates])
                    };

                    this.SheetData.Units.Add(temp);
                }
                catch(Exception ex)
                {
                    throw new UnitProcessingException(row[config.UnitName].ToString(), ex);
                }
            }
        }
    }
}
