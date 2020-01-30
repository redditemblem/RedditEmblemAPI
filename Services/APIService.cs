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
using System.Text.RegularExpressions;
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
                        Coordinates = new Coordinate(unit[config.Coordinates]),
                        Stats = BuildStatsDictionary(unit, config.Stats),
                        Inventory = BuildInventory(unit, config.Inventory)
                    };

                    this.SheetData.Units.Add(temp);
                }
                catch(Exception ex)
                {
                    throw new UnitProcessingException(row[config.UnitName].ToString(), ex);
                }
            }
        }

        private Dictionary<string, StatValue> BuildStatsDictionary(IList<string> unit, IList<StatConfig> config)
        {
            Dictionary<string, StatValue> stats = new Dictionary<string, StatValue>();

            foreach(StatConfig s in config)
            {
                StatValue temp = new StatValue();

                //Parse base value
                int val;
                if (!int.TryParse(unit[s.BaseValue], out val))
                    throw new PositiveIntegerException("", unit[s.BaseValue]);
                temp.BaseValue = val;

                //Parse modifiers list
                foreach(ModifierConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(unit[mod.Cell], out val))
                        throw new AnyIntegerException("", unit[mod.Cell]);
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                stats.Add(s.Name, temp);
            }
                

            return stats;
        }

        private IList<Item> BuildInventory(IList<string> unit, InventoryConfig config)
        {
            Regex usesRegex = new Regex(@"\([0-9]+\)"); //match uses ex. "(5)"
            Regex dropRegex = new Regex(@"\(D\)"); //match droppable ex. "(D)"
            IList<Item> inventory = new List<Item>();

            foreach (int slot in config.Slots)
            {
                string name = unit[slot];
                if (string.IsNullOrEmpty(name))
                {
                    inventory.Add(null);
                    continue;
                }
                    
                bool isDroppable = false;
                int uses = 0;

                //Search for droppable syntax
                Match dropMatch = dropRegex.Match(name);
                if (dropMatch.Success)
                {
                    isDroppable = true;
                    name = dropRegex.Replace(name, string.Empty);
                }

                //Search for uses syntax
                Match usesMatch = usesRegex.Match(name);
                if (usesMatch.Success)
                {
                    //Convert item use synatax to int
                    string u = usesMatch.Value.ToString();
                    u = u.Substring(1, u.Length - 2);
                    uses = int.Parse(u);
                    name = usesRegex.Replace(name, string.Empty);
                }

                inventory.Add(new Item() {
                    Name =  name.Trim(),
                    OriginalName = unit[slot],
                    IsDroppable = isDroppable,
                    Uses = uses
                });
            }

            //Find the equipped item and flag it
            if (!string.IsNullOrEmpty(unit[config.EquippedItem]))
            {
                Item equipped = inventory.FirstOrDefault(i => i.OriginalName == unit[config.EquippedItem]);
                if (equipped != null)
                    equipped.IsEquipped = true;
            }
            
            return inventory;
        }
    }
}
