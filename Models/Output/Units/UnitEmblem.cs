﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
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
        public List<UnitSkill> SyncSkills { get; set; }

        /// <summary>
        /// List of the emblem's engage skills.
        /// </summary>
        public List<UnitSkill> EngageSkills { get; set; }

        /// <summary>
        /// List of the emblem's engage weapons.
        /// </summary>
        public List<UnitInventoryItem> EngageWeapons { get; set; }

        /// <summary>
        /// List of the emblem's engage attacks.
        /// </summary>
        [JsonIgnore]
        public List<EngageAttack> EngageAttacksList { get; set; }

        #region JSON Serialization ONLY

        /// <summary>
        /// Only for JSON serialization. The name of the emblem.
        /// </summary>
        [JsonProperty]
        private string Name { get { return this.Emblem.Name; } }

        /// <summary>
        /// Only for JSON serialization. List of the emblem's engage attack names.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> EngageAttacks { get { return this.EngageAttacksList.Select(a => a.Name); } }

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

            this.SyncSkills = BuildUnitSkills(data, config.SyncSkills, systemData.Skills);
            this.EngageSkills = BuildUnitSkills(data, config.EngageSkills, systemData.Skills);

            List<string> engageAttacks = DataParser.List_Strings(data, config.EngageAttacks);
            this.EngageAttacksList = EngageAttack.MatchNames(systemData.EngageAttacks, engageAttacks);

            BuildItems(data, config, systemData.Items, systemData.Engravings);
        }

        /// <summary>
        /// Builds and returns a list of the unit's skills.
        /// </summary>
        private List<UnitSkill> BuildUnitSkills(IEnumerable<string> data, List<UnitSkillConfig> configs, IReadOnlyDictionary<string, Skill> skills)
        {
            List<UnitSkill> unitSkills = new List<UnitSkill>();
            foreach (UnitSkillConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Skill Name");
                if (string.IsNullOrEmpty(name)) continue;

                unitSkills.Add(new UnitSkill(data, config, skills));
            }

            return unitSkills;
        }

        private void BuildItems(IEnumerable<string> data, UnitEmblemConfig config, IReadOnlyDictionary<string, Item> items, IReadOnlyDictionary<string, Engraving> engravings)
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
