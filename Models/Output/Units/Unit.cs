using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
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
        /// List of the status conditions the unit possesses.
        /// </summary>
        public List<UnitStatus> StatusConditions { get; set; }

        /// <summary>
        /// Container for information about the unit's inventory.
        /// </summary>
        public UnitInventory Inventory { get; set; }

        /// <summary>
        /// List of the skills the unit possesses.
        /// </summary>
        public List<UnitSkill> Skills { get; set; }

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

        #endregion JSON Serialization Only

        #endregion Attributes

        #region Constants

        private static Regex nonSpacingMarkRegex = new Regex(@"\p{Mn}");
        private static Regex unitNumberRegex = new Regex(@"\s([0-9]+$)"); //matches digits at the end of a string (ex. "Swordmaster _05_")

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Unit(UnitsConfig config, IEnumerable<string> data, SystemInfo system)
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

            string affiliation = DataParser.String(data, config.Affiliation, "Affiliation");
            this.AffiliationObj = System.Affiliation.MatchName(system.Affiliations, affiliation);

            //Classes handling. If the system does not use classes, fall back on the MovementType field
            this.ClassList = BuildClasses(data, config.Classes, system.Classes);
            if (this.ClassList.Count == 0)
                this.UnitMovementType = DataParser.String(data, config.MovementType, "Movement Type");
            MatchTags(system.Tags);

            this.Skills =  BuildUnitSkills(data, config.Skills, system.Skills);
            this.WeaponRanks = BuildWeaponRanks(data, config.WeaponRanks, system.Constants.WeaponRanks.Any());
            this.StatusConditions = BuildUnitStatusConditions(data, config.StatusConditions, system.StatusConditions);
            this.Battalion = BuildBattalion(data, config.Battalion, system.Battalions);
            this.Emblem = BuildUnitEmblem(data, config.Emblem, system);

            this.Inventory = BuildUnitInventory(data, config.Inventory, system);
        }

        #region Build Functions

        /// <summary>
        /// Builds and returns a list of the unit's skills.
        /// </summary>
        private List<UnitSkill> BuildUnitSkills(IEnumerable<string> data, List<UnitSkillConfig> configs, IDictionary<string, Skill> skills)
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

        /// <summary>
        /// Builds and returns the unit's dictionary of weapon rank types/letters.
        /// </summary>
        /// <param name="validateWeaponRankLetters">Flag indicating if weapon rank types should have an accompanying letter.</param>
        /// <exception cref="WeaponRankMissingLetterException"></exception>
        /// <exception cref="NonUniqueObjectNameException"></exception>
        private IDictionary<string, string> BuildWeaponRanks(IEnumerable<string> data, List<UnitWeaponRanksConfig> config, bool validateWeaponRankLetters)
        {
            IDictionary<string, string> weaponRanks = new Dictionary<string, string>();
            foreach (UnitWeaponRanksConfig rank in config)
            {
                string rankType;
                if (!string.IsNullOrEmpty(rank.SourceName)) rankType = rank.SourceName;
                else rankType = DataParser.OptionalString(data, rank.Type, "Weapon Rank Type");

                string rankLetter = DataParser.OptionalString(data, rank.Rank, "Weapon Rank Letter");

                if (!string.IsNullOrEmpty(rankType))
                {
                    if (validateWeaponRankLetters && string.IsNullOrEmpty(rankLetter))
                        throw new WeaponRankMissingLetterException(rankType);
                    if (!weaponRanks.TryAdd(rankType, rankLetter))
                        throw new NonUniqueObjectNameException("weapon rank", rankType);
                }
            }

            return weaponRanks;
        }

        /// <summary>
        /// Builds and returns a list of the unit's status conditions.
        /// </summary>
        private List<UnitStatus> BuildUnitStatusConditions(IEnumerable<string> data, List<UnitStatusConditionConfig> configs, IDictionary<string, StatusCondition> statuses)
        {
            List<UnitStatus> statusConditions = new List<UnitStatus>();
            foreach (UnitStatusConditionConfig config in configs)
            {
                string name = DataParser.OptionalString(data, config.Name, "Status Condition Name");
                if (string.IsNullOrEmpty(name)) continue;

                statusConditions.Add(new UnitStatus(data, config, statuses));
            }

            return statusConditions;
        }

        /// <summary>
        /// Builds and returns the unit's inventory container.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// <item>WeaponRanks</item>
        /// <item>Emblem</item>
        /// </list>
        /// </remarks>
        private UnitInventory BuildUnitInventory(IEnumerable<string> data, InventoryConfig config, SystemInfo system)
        {
            UnitInventory inventory = new UnitInventory(config, system, data, this.Emblem);

            foreach (UnitInventoryItem item in inventory.Items)
            {
                //Check if the item can be equipped
                string unitRank;
                if (this.WeaponRanks.TryGetValue(item.Item.Category, out unitRank))
                {
                    if (string.IsNullOrEmpty(unitRank)
                     || string.IsNullOrEmpty(item.Item.WeaponRank)
                     || system.Constants.WeaponRanks.IndexOf(unitRank) >= system.Constants.WeaponRanks.IndexOf(item.Item.WeaponRank))
                        item.CanEquip = true;
                }
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && !item.Item.UtilizedStats.Any())
                {
                    item.CanEquip = true;
                }

            }

            UnitInventoryItem primaryEquipped = inventory.GetPrimaryEquippedItem();
            if (primaryEquipped != null)
            {
                //Check if we need to apply weapon rank bonuses for the primary equipped item
                if (this.WeaponRanks.ContainsKey(primaryEquipped.Item.Category))
                {
                    string unitRank;
                    this.WeaponRanks.TryGetValue(primaryEquipped.Item.Category, out unitRank);

                    WeaponRankBonus rankBonus = system.WeaponRankBonuses.FirstOrDefault(b => b.Category == primaryEquipped.Item.Category && b.Rank == unitRank);
                    if (rankBonus != null)
                    {
                        foreach (KeyValuePair<string, int> mod in rankBonus.CombatStatModifiers)
                        {
                            ModifiedStatValue stat = this.Stats.MatchCombatStatName(mod.Key);
                            stat.Modifiers.Add($"{primaryEquipped.Item.Category} {unitRank} Rank Bonus", mod.Value);
                        }

                        foreach (KeyValuePair<string, int> mod in rankBonus.StatModifiers)
                        {
                            ModifiedStatValue stat = this.Stats.MatchGeneralStatName(mod.Key);
                            stat.Modifiers.Add($"{primaryEquipped.Item.Category} {unitRank} Rank Bonus", mod.Value);
                        }
                    }
                }
            }

            //Apply equipped stat modifiers
            foreach (UnitInventoryItem equipped in inventory.GetAllEquippedItems())
            {
                foreach (KeyValuePair<string, int> mod in equipped.Item.EquippedStatModifiers)
                {
                    ModifiedStatValue stat = this.Stats.MatchGeneralStatName(mod.Key);
                    stat.Modifiers.Add($"{equipped.Item.Name} (Eqp)", mod.Value);
                }
            }

            //Apply inventory stat modifiers for all nonequipped items
            foreach (UnitInventoryItem inv in inventory.GetAllUnequippedItems())
            {
                foreach (KeyValuePair<string, int> mod in inv.Item.InventoryStatModifiers)
                {
                    ModifiedStatValue stat = this.Stats.MatchGeneralStatName(mod.Key);
                    stat.Modifiers.Add($"{inv.Item.Name} (Inv)", mod.Value);
                }
            }

            return inventory;
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>Class</c> from <paramref name="classes"/>.
        /// </summary>
        /// /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Tags</item>
        /// </list>
        /// </remarks>
        /// <exception cref="UnmatchedClassException"></exception>
        private List<Class> BuildClasses(IEnumerable<string> data, List<int> indexes, IDictionary<string, Class> classes)
        {
            List<Class> unitClasses = new List<Class>();

            foreach (int index in indexes)
            {
                string name = DataParser.OptionalString(data, index, "Class Name");
                if (string.IsNullOrEmpty(name))
                    continue;

                Class match = Class.MatchName(classes, name);
                unitClasses.Add(match);

                //Append class tags to unit's tags
                this.Tags = this.Tags.Union(match.Tags).Distinct().ToList();
            }

            //If we have class fields configured, error if we found no values
            if (indexes.Count > 0 && !unitClasses.Any())
                throw new Exception("Unit must have at least one class defined.");

            return unitClasses;
        }

        /// <summary>
        /// Builds and returns the unit's battalion.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Stats</item>
        /// </list>
        /// </remarks>
        private UnitBattalion BuildBattalion(IEnumerable<string> data, UnitBattalionConfig config, IDictionary<string, Battalion> battalions)
        {
            if (config == null) return null;

            string name = DataParser.OptionalString(data, config.Battalion, "Battalion");
            if (string.IsNullOrEmpty(name)) return null;

            UnitBattalion battalion = new UnitBattalion(config, data, battalions);

            //Apply any stat modifiers from the battalion
            foreach (KeyValuePair<string, int> mod in battalion.BattalionObj.StatModifiers)
            {
                ModifiedStatValue stat = this.Stats.MatchGeneralStatName(mod.Key);
                stat.Modifiers.Add(battalion.BattalionObj.Name, mod.Value);
            }

            return battalion;
        }

        /// <summary>
        /// Builds and returns the unit's emblem.
        /// </summary>
        /// <remarks>
        /// Depends on the following being built beforehand:
        /// <list type="bullet">
        /// <item>Sprite</item>
        /// </list>
        /// </remarks>
        private UnitEmblem BuildUnitEmblem(IEnumerable<string> data, UnitEmblemConfig config, SystemInfo systemData)
        {
            if (config == null) return null;

            string name = DataParser.OptionalString(data, config.Name, "Emblem");
            if (string.IsNullOrEmpty(name)) return null;

            UnitEmblem emblem = new UnitEmblem(config, data, systemData);

            //Set unit aura
            if (emblem.IsEngaged && !string.IsNullOrEmpty(emblem.Emblem.EngagedUnitAura))
                this.Sprite.Aura = emblem.Emblem.EngagedUnitAura;

            return emblem;
        }

        #endregion Build Functions

        #region Match Functions

        /// <summary>
        /// Dependent on <c>this.Tags</c> already being built. Iterates through the values in <c>this.Tags</c> and attempts to match them a <c>Tag</c> from <paramref name="tags"/>.
        /// </summary>
        private void MatchTags(IDictionary<string, Tag> tags)
        {
            if (tags.Count == 0) return;

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

        private string RemoveDiacritics(string name)
        {
            string normalizedText = name.Normalize(NormalizationForm.FormD);
            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }

        /// <summary>
        /// Regex evaluates <paramref name="name"/> to extract the unit number. Returns it if one is found, else returns an empty string.
        /// </summary>
        private string ExtractUnitNumberFromName(string name)
        {
            Match numberMatch = unitNumberRegex.Match(name);
            if (numberMatch.Success)
                return numberMatch.Value.Trim();
            return string.Empty;
        }

        /// <summary>
        /// Returns the unit's movement type, and accounts for things like effects and classes.
        /// </summary>
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
        public IEnumerable<Skill> GetFullSkillsList()
        {
            IEnumerable<Skill> skills = this.Skills.Select(s => s.SkillObj);

            //Union w/ emblem skills
            if(this.Emblem != null)
            {
                skills = skills.Union(this.Emblem.SyncSkills.Select(s => s.SkillObj));
                if (this.Emblem.IsEngaged)
                    skills = skills.Union(this.Emblem.EngageSkills.Select(s => s.SkillObj));
            }

            return skills;
        }

        #region Static Functions

        /// <summary>
        /// Iterates through the data in <paramref name="config"/>'s <c>Query</c> and builds a <c>Unit</c> from each valid row.
        /// </summary>
        /// <exception cref="UnitProcessingException"></exception>
        public static List<Unit> BuildList(UnitsConfig config, SystemInfo system)
        {
            List<Unit> units = new List<Unit>();
            if (config == null || config.Query == null)
                return units;

            //Create units
            foreach (List<object> row in config.Query.Data)
            {
                string name = string.Empty;
                try
                {
                    //Convert objects to strings
                    IEnumerable<string> unit = row.Select(r => r.ToString());
                    name = DataParser.OptionalString(unit, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (units.Any(u => u.Name == name))
                        throw new NonUniqueObjectNameException("unit");

                    units.Add(new Unit(config, unit, system));
                }
                catch (Exception ex)
                {
                    throw new UnitProcessingException(name, ex);
                }
            }

            return units;
        }

        #endregion Static Functions
    }
}