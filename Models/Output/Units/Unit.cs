using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Models.Output.System.StatusConditions.Effects;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a single Unit.
    /// </summary>
    public class Unit
    {
        #region Attributes

        /// <summary>
        /// The unit's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The set of digits present at the end of the unit's <c>Name</c>, if any.
        /// </summary>
        public string UnitNumber { get; set; }

        /// <summary>
        /// The player that controls the unit.
        /// </summary>
        public string Player { get; set; }

        /// <summary>
        /// The unit's character application URL link.
        /// </summary>
        public string CharacterApplicationURL { get; set; }

        /// <summary>
        /// List of the unit's text fields.
        /// </summary>
        public List<string> TextFields { get; set; }

        /// <summary>
        /// Container for information about rendering a unit.
        /// </summary>
        public UnitSpriteData Sprite { get; set; }

        /// <summary>
        /// Container for information about the unit's location on the map.
        /// </summary>
        public UnitLocationData Location { get; set; }

        /// <summary>
        /// A list of the unit's classes.
        /// </summary>
        [JsonIgnore]
        public List<Class> ClassList { get; set; }

        /// <summary>
        /// The unit's movement type. Only used if classes are not provided.
        /// </summary>
        private string UnitMovementType { get; set; }

        /// <summary>
        /// The unit's affiliation.
        /// </summary>
        [JsonIgnore]
        public Affiliation AffiliationObj { get; set; }

        /// <summary>
        /// Container for information about the unit's raw numbers.
        /// </summary>
        public UnitStatsData Stats { get; set; }

        /// <summary>
        /// List of the unit's tags.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of how the unit behaves.
        /// </summary>
        public string Behavior { get; set; }

        /// <summary>
        /// Collection of the unit's weapon ranks.
        /// </summary>
        public IDictionary<string, string> WeaponRanks { get; set; }

        /// <summary>
        /// List of the statuses the unit has.
        /// </summary>
        public List<UnitStatus> StatusConditions { get; set; }

        /// <summary>
        /// Container for information about the unit's inventory.
        /// </summary>
        public UnitInventory Inventory { get; set; }

        /// <summary>
        /// List of the skills the unit possesses.
        /// </summary>
        [JsonIgnore]
        public List<Skill> SkillList { get; set; }

        /// <summary>
        /// Container for information about a unit's battalion.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UnitBattalion Battalion { get; set; }

        /// <summary>
        /// Container for information about a unit's emblem.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UnitEmblem Emblem { get; set; }

        /// <summary>
        /// Container for information about a unit's movement/item ranges.
        /// </summary>
        public UnitRangeData Ranges { get; set; }

        #region JSON Serialization Only

        /// <summary>
        /// The unit's name, with special characters swapped for alphabetical ones to help with searching.
        /// </summary>
        [JsonProperty]
        private string NormalizedName { get; set; }

        /// <summary>
        /// The unit's movement type
        /// </summary>
        [JsonProperty]
        private string MovementType { get { return GetUnitMovementType(); } }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's classes.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Classes { get { return this.ClassList.Select(c => c.Name); } }

        /// <summary>
        /// Only for JSON serialization. The unit's affiliation name.
        /// </summary>
        [JsonProperty]
        private string Affiliation { get { return AffiliationObj.Name; } }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's skills.
        /// </summary>
        [JsonProperty]
        private IEnumerable<string> Skills { get { return this.SkillList.Select(c => c.Name); } }

        #endregion JSON Serialization Only

        #endregion Attributes

        #region Constants

        private static Regex nonSpacingMarkRegex = new Regex(@"\p{Mn}");
        private static Regex unitNumberRegex = new Regex(@"\s([0-9]+$)"); //matches digits at the end of a string (ex. "Swordmaster _05_")

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Unit(UnitsConfig config, IEnumerable<string> data, SystemInfo systemData)
        {
            //Basic fields
            this.Name = DataParser.String(data, config.Name, "Name");
            this.NormalizedName = RemoveDiacritics(this.Name);
            this.UnitNumber = ExtractUnitNumberFromName(this.Name);
            this.Player = DataParser.OptionalString(data, config.Player, "Player");
            this.CharacterApplicationURL = DataParser.OptionalString_URL(data, config.CharacterApplicationURL, "Character Application URL");
            this.TextFields = DataParser.List_Strings(data, config.TextFields);
            this.Tags = DataParser.List_StringCSV(data, config.Tags).Distinct().ToList();
            this.Behavior = DataParser.OptionalString(data, config.Behavior, "Behavior");

            //Complex container objects
            this.Sprite = new UnitSpriteData(config, data);
            this.Location = new UnitLocationData(config, data);
            this.Stats = new UnitStatsData(config, data);
            this.Ranges = new UnitRangeData();

            MatchAffiliation(data, config.Affiliation, systemData.Affiliations);
            BuildClasses(data, config.Classes, systemData.Classes);

            //If the system does not use classes, fall back on the MovementType attribute
            if (this.ClassList.Count == 0)
                this.UnitMovementType = DataParser.String(data, config.MovementType, "Movement Type");

            IEnumerable<string> skillNames = DataParser.List_Strings(data, config.Skills);
            this.SkillList = Skill.MatchNames(systemData.Skills, skillNames);

            BuildWeaponRanks(data, config.WeaponRanks, systemData.WeaponRanks.Any());
            BuildStatusConditions(data, config.StatusConditions, systemData.StatusConditions);
            BuildBattalion(data, config.Battalion, systemData.Battalions);

            //If we have loaded tags, attempt to match. If not, just keep our plaintext list.
            if (systemData.Tags.Count > 0)
                MatchTags(systemData.Tags);

            BuildEmblem(data, config.Emblem, systemData); //need to come after tags for aura application
            BuildInventory(data, config.Inventory, systemData); //dependent on emblems
        }

        #region Build Functions

        private void BuildWeaponRanks(IEnumerable<string> data, List<UnitWeaponRanksConfig> config, bool systemUsesWeaponRanks)
        {
            this.WeaponRanks = new Dictionary<string, string>();

            foreach (UnitWeaponRanksConfig rank in config)
            {
                string rankType;
                if (!string.IsNullOrEmpty(rank.SourceName)) rankType = rank.SourceName;
                else rankType = DataParser.OptionalString(data, rank.Type, "Weapon Rank Type");

                string rankLetter = DataParser.OptionalString(data, rank.Rank, "Weapon Rank Letter");

                if (!string.IsNullOrEmpty(rankType))
                {
                    if (systemUsesWeaponRanks && string.IsNullOrEmpty(rankLetter))
                        throw new WeaponRankMissingLetterException(rankType);
                    if (!this.WeaponRanks.TryAdd(rankType, rankLetter))
                        throw new NonUniqueObjectNameException("weapon rank", rankType);
                }
            }
        }

        private void BuildStatusConditions(IEnumerable<string> data, List<UnitStatusConditionConfig> configs, IDictionary<string, StatusCondition> statuses)
        {
            this.StatusConditions = new List<UnitStatus>();

            foreach (UnitStatusConditionConfig config in configs)
            {
                //Skip blank cells
                string name = DataParser.OptionalString(data, config.Name, "Status Condition Name");
                if (string.IsNullOrEmpty(name))
                    continue;

                UnitStatus status = new UnitStatus(data, config, statuses);
                this.StatusConditions.Add(status);
            }
        }

        private void BuildInventory(IEnumerable<string> data, InventoryConfig config, SystemInfo system)
        {
            this.Inventory = new UnitInventory(config, data, system.Items, system.Engravings, this.Emblem);

            foreach (UnitInventoryItem item in this.Inventory.Items)
            {
                //Check if the item can be equipped
                string unitRank;
                if (this.WeaponRanks.TryGetValue(item.Item.Category, out unitRank))
                {
                    if (string.IsNullOrEmpty(unitRank)
                     || string.IsNullOrEmpty(item.Item.WeaponRank)
                     || system.WeaponRanks.IndexOf(unitRank) >= system.WeaponRanks.IndexOf(item.Item.WeaponRank))
                        item.CanEquip = true;
                }
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && !item.Item.UtilizedStats.Any())
                {
                    item.CanEquip = true;
                }

            }

            UnitInventoryItem primaryEquipped = this.Inventory.GetPrimaryEquippedItem();
            if (primaryEquipped != null)
            {
                //Check if we need to apply weapon rank bonuses for the primary equipped item
                if (this.WeaponRanks.ContainsKey(primaryEquipped.Item.Category))
                {
                    string unitRank;
                    this.WeaponRanks.TryGetValue(primaryEquipped.Item.Category, out unitRank);

                    WeaponRankBonus bonus = system.WeaponRankBonuses.FirstOrDefault(b => b.Category == primaryEquipped.Item.Category && b.Rank == unitRank);
                    if (bonus != null)
                    {
                        foreach (string stat in bonus.CombatStatModifiers.Keys)
                        {
                            ModifiedStatValue mods = this.Stats.MatchCombatStatName(stat);
                            mods.Modifiers.Add($"{primaryEquipped.Item.Category} {unitRank} Rank Bonus", bonus.CombatStatModifiers[stat]);
                        }

                        foreach (string stat in bonus.StatModifiers.Keys)
                        {
                            ModifiedStatValue mods = this.Stats.MatchGeneralStatName(stat);
                            mods.Modifiers.Add($"{primaryEquipped.Item.Category} {unitRank} Rank Bonus", bonus.StatModifiers[stat]);
                        }
                    }
                }
            }

            //Apply equipped stat modifiers
            foreach (UnitInventoryItem equipped in this.Inventory.GetAllEquippedItems())
            {
                foreach (string stat in equipped.Item.EquippedStatModifiers.Keys)
                {
                    ModifiedStatValue mods = this.Stats.MatchGeneralStatName(stat);
                    mods.Modifiers.Add($"{equipped.Item.Name} (Eqp)", equipped.Item.EquippedStatModifiers[stat]);
                }
            }

            //Apply inventory stat modifiers for all nonequipped items
            foreach (UnitInventoryItem inv in this.Inventory.GetAllUnequippedItems())
            {
                foreach (string stat in inv.Item.InventoryStatModifiers.Keys)
                {
                    ModifiedStatValue mods = this.Stats.MatchGeneralStatName(stat);
                    mods.Modifiers.Add($"{inv.Item.Name} (Inv)", inv.Item.InventoryStatModifiers[stat]);
                }
            }
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>Class</c> from <paramref name="classes"/>.
        /// </summary>
        /// <exception cref="UnmatchedClassException"></exception>
        private void BuildClasses(IEnumerable<string> data, List<int> indexes, IDictionary<string, Class> classes)
        {
            this.ClassList = new List<Class>();

            foreach (int index in indexes)
            {
                string className = DataParser.OptionalString(data, index, "Class Name");
                if (string.IsNullOrEmpty(className))
                    continue;

                Class match = Class.MatchName(classes, className);
                this.ClassList.Add(match);

                //Append class tags to unit's tags
                this.Tags = this.Tags.Union(match.Tags).Distinct().ToList();
            }

            if (indexes.Count > 0 && !this.ClassList.Any())
                throw new Exception("Unit must have at least one class defined.");
        }

        private void BuildBattalion(IEnumerable<string> data, UnitBattalionConfig config, IDictionary<string, Battalion> battalions)
        {
            if (config == null)
                return;

            string name = DataParser.OptionalString(data, config.Battalion, "Battalion");
            if (string.IsNullOrEmpty(name))
                return;

            this.Battalion = new UnitBattalion(config, data, battalions);

            //Apply any stat modifiers from the battalion
            Battalion battalion = this.Battalion.BattalionObj;
            foreach (string stat in battalion.StatModifiers.Keys)
            {
                ModifiedStatValue mods = this.Stats.MatchGeneralStatName(stat);
                mods.Modifiers.Add(battalion.Name, battalion.StatModifiers[stat]);
            }
        }

        private void BuildEmblem(IEnumerable<string> data, UnitEmblemConfig config, SystemInfo systemData)
        {
            if (config == null) return;

            string name = DataParser.OptionalString(data, config.Name, "Emblem");
            if (string.IsNullOrWhiteSpace(name)) return;

            this.Emblem = new UnitEmblem(config, data, systemData);

            if (this.Emblem.IsEngaged && !string.IsNullOrEmpty(this.Emblem.Emblem.EngagedUnitAura))
                this.Sprite.Aura = this.Emblem.Emblem.EngagedUnitAura;
        }

        #endregion Build Functions

        #region Match Functions

        private void MatchAffiliation(IEnumerable<string> data, int affiliationIndex, IDictionary<string, Affiliation> affiliations)
        {
            string name = DataParser.String(data, affiliationIndex, "Affiliation");

            this.AffiliationObj = System.Affiliation.MatchName(affiliations, name);
        }

        /// <summary>
        /// MUST BE RUN AFTER TAGS ARE BUILT. Iterates through the values in <c>this.Tags</c> and attempts to match them a <c>Tag</c> from <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags"></param>
        private void MatchTags(IDictionary<string, Tag> tags)
        {
            List<Tag> matched = Tag.MatchNames(tags, this.Tags);
            
            //Apply the unit aura from the first valid tag encountered
            if(string.IsNullOrEmpty(this.Sprite.Aura))
            {
                Tag aura = matched.FirstOrDefault(t => !string.IsNullOrEmpty(t.UnitAura));
                if(aura != null)
                    this.Sprite.Aura = aura.UnitAura;
            }
        }

        #endregion Match Functions

        public static string RemoveDiacritics(string name)
        {
            string normalizedText = name.Normalize(NormalizationForm.FormD);
            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }

        private string ExtractUnitNumberFromName(string name)
        {
            Match numberMatch = unitNumberRegex.Match(name);
            if (numberMatch.Success)
                return numberMatch.Value.Trim();
            return String.Empty;
        }

        public string GetUnitMovementType()
        {
            OverrideMovementTypeEffect overrideMovementType = this.StatusConditions.SelectMany(s => s.StatusObj.Effects).OfType<OverrideMovementTypeEffect>().FirstOrDefault();
            if (overrideMovementType != null)
                return overrideMovementType.MovementType;

            if (this.ClassList.Count > 0)
                return this.ClassList.First().MovementType;
            else return this.UnitMovementType;
        }

        /// <summary>
        /// Returns complete list of skills on the unit, including skills from subitems like Emblems.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Skill> GetSkills()
        {
            IEnumerable<Skill> skills = this.SkillList;

            //Union w/ emblem skills
            if(this.Emblem != null)
            {
                skills = skills.Union(this.Emblem.SyncSkillsList);
                if (this.Emblem.IsEngaged)
                    skills = skills.Union(this.Emblem.EngageSkillsList);
            }

            return skills;
        }
    }
}