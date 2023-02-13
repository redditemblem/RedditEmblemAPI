using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    public class UnitEmblem
    {
        #region Attributes

        /// <summary>
        /// The <c>Emblem</c> object.
        /// </summary>
        [JsonIgnore]
        public Emblem Emblem { get; set; }

        /// <summary>
        /// The bond level that the unit has with this emblem.
        /// </summary>
        public int BondLevel { get; set; }

        /// <summary>
        /// The current engage meter count.
        /// </summary>
        public int EngageMeterCount { get; set; }

        /// <summary>
        /// Flag indicating whether or not the unit is engaged with this emblem.
        /// </summary>
        public bool IsEngaged { get; set; }

        /// <summary>
        /// List of the emblem's sync skills.
        /// </summary>
        [JsonIgnore]
        public List<Skill> SyncSkillsList { get; set; }

        /// <summary>
        /// List of the emblem's engage skills.
        /// </summary>
        [JsonIgnore]
        public List<Skill> EngageSkillsList { get; set; }

        /// <summary>
        /// List of the emblem's engage weapons.
        /// </summary>
        public List<UnitInventoryItem> EngageWeapons { get; set; }

        #region JSON Serialization ONLY

        /// <summary>
        /// Only for JSON serialization. The name of the emblem.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Emblem.Name; } }

        /// <summary>
        /// Only for JSON serialization. List of the names of the emblem's sync skills.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> SyncSkills { get { return this.SyncSkillsList.Select(s => s.Name); } }

        /// <summary>
        /// Only for JSON serialization. List of the nmaes of the emblem's engage skills.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> EngageSkills { get { return this.EngageSkillsList.Select(s => s.Name); } }

        #endregion JSON Serialization ONLY

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitEmblem(UnitEmblemConfig config, IEnumerable<string> data, SystemInfo systemData)
        {
            string name = DataParser.String(data, config.Name, "Emblem");
            this.Emblem = Emblem.MatchName(systemData.Emblems, name);

            this.BondLevel = DataParser.OptionalInt_NonZeroPositive(data, config.BondLevel, "Emblem Bond Level", 0);
            this.EngageMeterCount = DataParser.OptionalInt_Positive(data, config.EngageMeterCount, "Engage Meter Count", -1);
            this.IsEngaged = DataParser.OptionalBoolean_YesNo(data, config.IsEngaged, "Is Engaged");

            IEnumerable<string> syncSkills = DataParser.List_Strings(data, config.SyncSkills);
            this.SyncSkillsList = Skill.MatchNames(systemData.Skills, syncSkills);

            IEnumerable<string> engageSkills = DataParser.List_Strings(data, config.EngageSkills);
            this.EngageSkillsList = Skill.MatchNames(systemData.Skills, engageSkills);

            BuildItems(data, config, systemData.Items, systemData.Engravings);
        }

        private void BuildItems(IEnumerable<string> data, UnitEmblemConfig config, IDictionary<string, Item> items, IDictionary<string, Engraving> engravings)
        {
            this.EngageWeapons = new List<UnitInventoryItem>();
            IEnumerable<string> weapons = DataParser.List_Strings(data, config.EngageWeapons);

            foreach (string weapon in weapons)
            {
                UnitInventoryItem item = new UnitInventoryItem(weapon, 0, new List<string>(), items, engravings);
                item.CanEquip = true; //can always equip engage items

                this.EngageWeapons.Add(item);
            }
        }
    }
}
