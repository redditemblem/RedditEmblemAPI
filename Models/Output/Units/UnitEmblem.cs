using Newtonsoft.Json;
using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Match;
using RedditEmblemAPI.Models.Output.System.Skills;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitEmblem"/>
    public interface IUnitEmblem
    {
        /// <inheritdoc cref="UnitEmblem.Emblem"/>
        IEmblem Emblem { get; }

        /// <inheritdoc cref="UnitEmblem.BondLevel"/>
        int BondLevel { get; }

        /// <inheritdoc cref="UnitEmblem.EngageMeterCount"/>
        int EngageMeterCount { get; }

        /// <inheritdoc cref="UnitEmblem.IsEngaged"/>
        bool IsEngaged { get; }

        /// <inheritdoc cref="UnitEmblem.SyncSkills"/>
        List<IUnitSkill> SyncSkills { get; }

        /// <inheritdoc cref="UnitEmblem.EngageSkills"/>
        List<IUnitSkill> EngageSkills { get; }

        /// <inheritdoc cref="UnitEmblem.EngageWeapons"/>
        List<IUnitInventoryItem> EngageWeapons { get; }

        /// <inheritdoc cref="UnitEmblem.EngageAttacks"/>
        List<IEngageAttack> EngageAttacks { get; }
    }

    #endregion Interface

    public class UnitEmblem : IUnitEmblem
    {
        #region Attributes

        /// <summary>
        /// The <c>Emblem</c> object.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(MatchableNameConverter))]
        public IEmblem Emblem { get; set; }

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
        public List<IUnitSkill> SyncSkills { get; set; }

        /// <summary>
        /// List of the emblem's engage skills.
        /// </summary>
        public List<IUnitSkill> EngageSkills { get; set; }

        /// <summary>
        /// List of the emblem's engage weapons.
        /// </summary>
        public List<IUnitInventoryItem> EngageWeapons { get; set; }

        /// <summary>
        /// List of the emblem's engage attacks.
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(MatchableNameConverter))]
        public List<IEngageAttack> EngageAttacks { get; set; }

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitEmblem(UnitEmblemConfig config, IEnumerable<string> data, SystemInfo systemData)
        {
            string name = DataParser.String(data, config.Name, "Emblem");
            this.Emblem = System.Emblem.MatchName(systemData.Emblems, name);

            this.BondLevel = DataParser.OptionalInt_NonZeroPositive(data, config.BondLevel, "Emblem Bond Level", 0);
            this.EngageMeterCount = DataParser.OptionalInt_Positive(data, config.EngageMeterCount, "Engage Meter Count", -1);
            this.IsEngaged = DataParser.OptionalBoolean_YesNo(data, config.IsEngaged, "Is Engaged");

            this.SyncSkills = BuildUnitSkills(data, config.SyncSkills, systemData.Skills);
            this.EngageSkills = BuildUnitSkills(data, config.EngageSkills, systemData.Skills);

            List<string> engageAttacks = DataParser.List_Strings(data, config.EngageAttacks);
            this.EngageAttacks = EngageAttack.MatchNames(systemData.EngageAttacks, engageAttacks);

            BuildItems(data, config, systemData.Items, systemData.Engravings);
        }

        /// <summary>
        /// Builds and returns a list of the unit's skills.
        /// </summary>
        private List<IUnitSkill> BuildUnitSkills(IEnumerable<string> data, List<UnitSkillConfig> configs, IDictionary<string, ISkill> skills)
        {
            List<IUnitSkill> unitSkills = new List<IUnitSkill>();
            foreach (UnitSkillConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Skill Name");
                if (string.IsNullOrEmpty(name)) continue;

                unitSkills.Add(new UnitSkill(data, config, skills));
            }

            return unitSkills;
        }

        private void BuildItems(IEnumerable<string> data, UnitEmblemConfig config, IDictionary<string, IItem> items, IDictionary<string, IEngraving> engravings)
        {
            this.EngageWeapons = new List<IUnitInventoryItem>();
            IEnumerable<string> weapons = DataParser.List_Strings(data, config.EngageWeapons);

            foreach (string weapon in weapons)
            {
                IUnitInventoryItem item = new UnitInventoryItem(weapon, 0, new List<string>(), items, engravings);
                item.CanEquip = true; //can always equip engage items

                this.EngageWeapons.Add(item);
            }
        }
    }
}
