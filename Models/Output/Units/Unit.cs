using NCalc;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Skills;
using RedditEmblemAPI.Models.Output.System.StatusConditions;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Units
{
    /// <summary>
    /// Object representing a single Unit.
    /// </summary>
    public class Unit
    {
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
        /// The sprite image URL for the unit.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The portrait image URL for the unit.
        /// </summary>
        public string PortraitURL { get; set; }

        /// <summary>
        /// List of the unit's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

        /// <summary>
        /// Unparsed, raw coordinate string value.
        /// </summary>
        [JsonIgnore]
        public string CoordinateString { get; set; }

        /// <summary>
        /// The unit's location on the map.
        /// </summary>
        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// The unit's current level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// A list of the unit's classes.
        /// </summary>
        [JsonIgnore]
        public IList<Class> ClassList { get; set; }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's classes.
        /// </summary>
        [JsonProperty]
        private IList<string> Classes { get { return this.ClassList.Select(c => c.Name).ToList();  } }

        /// <summary>
        /// The unit's movement type. Only used if classes are not provided.
        /// </summary>
        public string MovementType { get; set; }

        /// <summary>
        /// The unit's affiliation.
        /// </summary>
        [JsonIgnore]
        public Affiliation AffiliationObj { get; set; }

        /// <summary>
        /// Only for JSON serialization. The unit's affiliation name.
        /// </summary>
        [JsonProperty]
        private string Affiliation { get { return AffiliationObj.Name; } }

        /// <summary>
        /// The unit's earned experience.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// The amount of currency the unit has in their wallet.
        /// </summary>
        public int HeldCurrency { get; set; }

        /// <summary>
        /// Container object for HP values.
        /// </summary>
        public HP HP { get; set; }

        /// <summary>
        /// List of the unit's tags.
        /// </summary>
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Description of how the unit behaves.
        /// </summary>
        public string Behavior { get; set; }

        /// <summary>
        /// Collection of the unit's calculated combat stats.
        /// </summary>
        public IDictionary<string, ModifiedStatValue> CombatStats { get; set; }

        /// <summary>
        /// Collection of the unit's system stats.
        /// </summary>
        public IDictionary<string, ModifiedStatValue> SystemStats { get; set; }

        /// <summary>
        /// Collection of the unit's stat values.
        /// </summary>
        public IDictionary<string, ModifiedStatValue> Stats { get; set; }

        /// <summary>
        /// Collection of the unit's weapon ranks.
        /// </summary>
        public IDictionary<string, string> WeaponRanks { get; set; }

        /// <summary>
        /// List of the statuses the unit has.
        /// </summary>
        public IList<UnitStatus> StatusConditions { get; set; }

        /// <summary>
        /// List of the items the unit is carrying.
        /// </summary>
        public IList<UnitInventoryItem> Inventory { get; set; }

        /// <summary>
        /// List of the skills the unit possesses.
        /// </summary>
        [JsonIgnore]
        public IList<Skill> SkillList { get; set; }

        /// <summary>
        /// Only for JSON serialization. A list of the unit's skills.
        /// </summary>
        [JsonProperty]
        private IList<string> Skills { get { return this.SkillList.Select(c => c.Name).ToList(); } }

        #region Pair_Ups

        /// <summary>
        /// The <c>Unit</c> paired with the unit, if any.
        /// </summary>
        [JsonIgnore]
        public Unit PairedUnitObj { get; set; }

        //// <summary>
        /// Only for JSON serialization. Returns the name of the <c>PairedUnit</c>. If <c>PairedUnit</c> is null, returns an empty string.
        /// </summary>
        [JsonProperty]
        private string PairedUnit { get { return (this.PairedUnitObj == null ? string.Empty : this.PairedUnitObj.Name); } }

        /// <summary>
        /// Flag indicating if the unit is sitting in the back of a pair.
        /// </summary>
        public bool IsBackOfPair { get; set; }

        #endregion

        #region Movement_And_Range

        /// <summary>
        /// Flag indicating whether or not a unit's turn has been processed.
        /// </summary>
        public bool HasMoved { get; set; }

        /// <summary>
        /// The size of the unit in grid tiles. Defaults to 1.
        /// </summary>
        public int UnitSize { get; set; }

        /// <summary>
        /// The <c>Tile</c> that this unit is drawn at.
        /// </summary>
        [JsonIgnore]
        public Tile AnchorTile { get; set; }

        /// <summary>
        /// The <c>Tile</c> that this unit's range originates from.
        /// </summary>
        [JsonIgnore]
        public IList<Tile> OriginTiles { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of moving to.
        /// </summary>
        public IList<Coordinate> MovementRange { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of attacking.
        /// </summary>
        public IList<Coordinate> AttackRange { get; set; }

        /// <summary>
        /// List of tiles that the unit is capable of using a utility item on.
        /// </summary>
        public IList<Coordinate> UtilityRange { get; set; }

        #endregion

        #region Constants

        private static Regex unitNumberRegex = new Regex(@"\s([0-9]+$)"); //matches digits at the end of a string (ex. "Swordmaster _05_")

        private static Regex unitStatRegex = new Regex(@"{UnitStat\[([A-Za-z]+)\]}"); //match unit stat name
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat\[([A-Za-z]+)\]}"); //match weapon stat name

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Unit(UnitsConfig config, IList<string> data, SystemInfo systemData)
        {
            //Required fields
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", true);
            this.Level = ParseHelper.Int_NonZeroPositive(data, config.Level, "Level");
            this.CoordinateString = ParseHelper.SafeStringParse(data, config.Coordinate, "Coordinate", false);
            this.HP = new HP(data, config.HP.Current, config.HP.Maximum);

            //Optional fields
            this.Player = ParseHelper.SafeStringParse(data, config.Player, "Player", false);
            this.PortraitURL = ParseHelper.SafeStringParse(data, config.PortraitURL, "Portrait URL", false);
            this.UnitSize = ParseHelper.OptionalInt_NonZeroPositive(data, config.UnitSize, "Unit Size");
            this.HasMoved = (ParseHelper.SafeStringParse(data, config.HasMoved, "Has Moved", false) == "Yes");

            int experience = ParseHelper.OptionalInt_Positive(data, config.Experience, "Experience", -1);
            if(experience > -1) experience %= 100;
            this.Experience = experience;

            this.HeldCurrency = ParseHelper.OptionalInt_Positive(data, config.HeldCurrency, "Currency");
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
            this.Tags = ParseHelper.StringCSVParse(data, config.Tags);
            this.Behavior = ParseHelper.SafeStringParse(data, config.Behavior, "Behavior", false);

            this.OriginTiles = new List<Tile>();
            this.MovementRange = new List<Coordinate>();
            this.AttackRange = new List<Coordinate>();
            this.UtilityRange = new List<Coordinate>();

            //Find unit number
            Match numberMatch = unitNumberRegex.Match(this.Name);
            if (numberMatch.Success)
                this.UnitNumber = numberMatch.Value.Trim();

            //Match affiliation
            string affiliation = ParseHelper.SafeStringParse(data, config.Affiliation, "Affiliation", true);
            Affiliation affMatch;
            if (!systemData.Affiliations.TryGetValue(affiliation, out affMatch))
                throw new UnmatchedAffiliationException(affiliation);
            this.AffiliationObj = affMatch;
            affMatch.Matched = true;

            this.ClassList = new List<Class>();
            BuildClasses(data, config.Classes, systemData.Classes);

            //If the system does not use classes, fall back on the MovementType attribute
            if (this.ClassList.Count == 0)
                this.MovementType = ParseHelper.SafeStringParse(data, config.MovementType, "Movement Type", true);

            this.WeaponRanks = new Dictionary<string, string>();
            BuildWeaponRanks(data, config.WeaponRanks, systemData.WeaponRanks.Any());

            this.CombatStats = new Dictionary<string, ModifiedStatValue>();
            BuildCombatStats(data, config.CombatStats);

            this.SystemStats = new Dictionary<string, ModifiedStatValue>();
            BuildSystemStats(data, config.SystemStats);

            this.Stats = new Dictionary<string, ModifiedStatValue>();
            BuildStats(data, config.Stats);

            this.Inventory = new List<UnitInventoryItem>();
            BuildInventory(data, config.Inventory, systemData.Items, systemData.WeaponRanks, systemData.WeaponRankBonuses);

            this.SkillList = new List<Skill>();
            BuildSkills(data, config.Skills, systemData.Skills);

            this.StatusConditions = new List<UnitStatus>();
            BuildStatusConditions(data, config.StatusConditions, systemData.StatusConditions);

            //If we have loaded tags, attempt to match. If not, just keep our plaintext list.
            if(systemData.Tags.Count > 0)
                MatchTags(systemData.Tags);
        }

        #region Build Functions

        private void BuildWeaponRanks(IList<string> data, IList<UnitWeaponRanksConfig> config, bool systemUsesWeaponRanks)
        {
            foreach (UnitWeaponRanksConfig rank in config)
            {
                string rankType = ParseHelper.SafeStringParse(data, rank.Type, "Weapon Rank Type", false);
                string rankLetter = ParseHelper.SafeStringParse(data, rank.Rank, "Weapon Rank Letter", false);

                if (!string.IsNullOrEmpty(rankType))
                {
                    if (systemUsesWeaponRanks && string.IsNullOrEmpty(rankLetter))
                        throw new WeaponRankMissingLetterException(rankType);
                    if(!this.WeaponRanks.TryAdd(rankType, rankLetter))
                        throw new NonUniqueObjectNameException("weapon rank", rankType);
                }
            }
        }

        private void BuildSystemStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            foreach(ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.Int_Positive(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, string.Format("{0} {1}", stat.SourceName, mod.SourceName));
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.SystemStats.Add(stat.SourceName, temp);
            }
        }

        private void BuildStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.Int_Positive(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, string.Format("{0} {1}", stat.SourceName, mod.SourceName));
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.Stats.Add(stat.SourceName, temp);
            }
        }

        private void BuildStatusConditions(IList<string> data, IList<int> indexes, IDictionary<string, StatusCondition> statuses)
        {
            foreach(int index in indexes)
            {
                //Skip blank cells
                string name = ParseHelper.SafeStringParse(data, index, "Status Condition Name", false);
                if (string.IsNullOrEmpty(name))
                    continue;

                UnitStatus status = new UnitStatus(name, statuses);
                this.StatusConditions.Add(status);
            }
        }

        private void BuildInventory(IList<string> data, InventoryConfig config, IDictionary<string, Item> items, IList<string> weaponRanks, IList<WeaponRankBonus> weaponRankBonuses)
        {
            foreach (int index in config.Slots)
            {
                string name = ParseHelper.SafeStringParse(data, index, "Item Name", false);
                if (string.IsNullOrEmpty(name))
                {
                    this.Inventory.Add(null);
                    continue;
                }
                UnitInventoryItem item = new UnitInventoryItem(name, items);

                //Check if the item can be equipped
                string unitRank;
                if(this.WeaponRanks.TryGetValue(item.Item.Category, out unitRank))
                {
                    if (string.IsNullOrEmpty(unitRank)
                     || string.IsNullOrEmpty(item.Item.WeaponRank)
                     || weaponRanks.IndexOf(unitRank) >= weaponRanks.IndexOf(item.Item.WeaponRank))
                        item.CanEquip = true;
                }
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && !item.Item.UtilizedStats.Any())
                {
                    item.CanEquip = true;
                }

                this.Inventory.Add(item);
            }

            //Find the equipped item and flag it
            string equippedItemName = ParseHelper.SafeStringParse(data, config.EquippedItem, "Equipped Item", false);
            if (!string.IsNullOrEmpty(equippedItemName))
            {
                UnitInventoryItem equipped = this.Inventory.FirstOrDefault(i => i != null && i.FullName == equippedItemName);
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(equippedItemName);
                equipped.IsEquipped = true;

                //Apply equipped stat modifiers
                foreach (string stat in equipped.Item.EquippedStatModifiers.Keys)
                {
                    ModifiedStatValue mods;
                    if (!this.Stats.TryGetValue(stat, out mods))
                        throw new UnmatchedStatException(stat);
                    mods.Modifiers.Add(string.Format("{0} ({1})", equipped.FullName, "Eqp"), equipped.Item.EquippedStatModifiers[stat]);
                }

                //Check if we need to apply weapon rank bonuses for the equipped item
                if (this.WeaponRanks.ContainsKey(equipped.Item.Category))
                {
                    string unitRank;
                    this.WeaponRanks.TryGetValue(equipped.Item.Category, out unitRank);

                    WeaponRankBonus bonus = weaponRankBonuses.FirstOrDefault(b => b.Category == equipped.Item.Category && b.Rank == unitRank);
                    if(bonus != null)
                    {
                        foreach(string stat in bonus.CombatStatModifiers.Keys)
                        {
                            ModifiedStatValue mods;
                            if (!this.CombatStats.TryGetValue(stat, out mods))
                                throw new UnmatchedStatException(stat);
                            mods.Modifiers.Add($"{equipped.Item.Category} {unitRank} Rank Bonus", bonus.CombatStatModifiers[stat]);
                        }

                        foreach (string stat in bonus.StatModifiers.Keys)
                        {
                            ModifiedStatValue mods;
                            if (!this.Stats.TryGetValue(stat, out mods))
                                throw new UnmatchedStatException(stat);
                            mods.Modifiers.Add($"{equipped.Item.Category} {unitRank} Rank Bonus", bonus.StatModifiers[stat]);
                        }
                    }
                }
            }

            //Apply inventory stat modifiers for all nonequipped items
            foreach (UnitInventoryItem inv in this.Inventory.Where(i => i != null && !i.IsEquipped))
            {
                foreach (string stat in inv.Item.InventoryStatModifiers.Keys)
                {
                    ModifiedStatValue mods;
                    if (!this.Stats.TryGetValue(stat, out mods))
                        throw new UnmatchedStatException(stat);
                    mods.Modifiers.Add(string.Format("{0} ({1})", inv.Item.Name, "Inv"), inv.Item.InventoryStatModifiers[stat]);
                }
            }
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>Skill</c> from <paramref name="skills"/>.
        /// </summary>
        /// <exception cref="UnmatchedSkillException"></exception>
        private void BuildSkills(IList<string> data, IList<int> indexes, IDictionary<string, Skill> skills)
        {
            foreach (int index in indexes)
            {
                //Skip blank cells
                string name = ParseHelper.SafeStringParse(data, index, "Skill Name", false);
                if (string.IsNullOrEmpty(name))
                    continue;

                Skill match;
                if (!skills.TryGetValue(name, out match))
                    throw new UnmatchedSkillException(name);
                match.Matched = true;

                this.SkillList.Add(match);
            }
        }

        /// <summary>
        /// Iterates through the values in <paramref name="data"/> at <paramref name="indexes"/> and attempts to match them to a <c>Class</c> from <paramref name="classes"/>.
        /// </summary>
        /// <exception cref="UnmatchedClassException"></exception>
        private void BuildClasses(IList<string> data, IList<int> indexes, IDictionary<string, Class> classes)
        {
            foreach (int index in indexes)
            {
                string className = ParseHelper.SafeStringParse(data, index, "Class Name", false);
                if (string.IsNullOrEmpty(className))
                    continue;
                
                Class match;
                if (!classes.TryGetValue(className, out match))
                    throw new UnmatchedClassException(className);

                match.Matched = true;
                this.ClassList.Add(match);

                //Append class tags to unit's tags
                this.Tags = this.Tags.Union(match.Tags).Distinct().ToList();
            }

            if (indexes.Count > 0 && this.ClassList.Count < 1)
                throw new Exception("Unit must have at least one class defined.");
        }

        /// <summary>
        /// MUST BE RUN AFTER TAGS ARE BUILT. Iterates through the values in <c>this.Tags</c> and attempts to match them a <c>Tag</c> from <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags"></param>
        private void MatchTags(IDictionary<string, Tag> tags)
        {
            foreach (string tag in this.Tags)
            {
                Tag match;
                if (!tags.TryGetValue(tag, out match))
                    throw new UnmatchedTagException(tag);

                match.Matched = true;
            }
        }

        /// <summary>
        /// Adds the stats from <paramref name="stats"/> into <c>CombatStats</c>. Does NOT calculate their values.
        /// </summary>
        private void BuildCombatStats(IList<string> data, IList<CalculatedStatConfig> stats)
        {
            foreach (CalculatedStatConfig stat in stats)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, string.Format("{0} {1}", stat.SourceName, mod.SourceName));
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.CombatStats.Add(stat.SourceName, temp);
            }
        }

        #endregion

        /// <summary>
        /// Assembles and executes the equations in <paramref name="stats"/>.
        /// </summary>
        /// <param name="stats"></param>
        public void CalculateCombatStats(IList<CalculatedStatConfig> stats)
        {
            foreach (CalculatedStatConfig stat in stats)
            {
                string equation = stat.Equation;
                UnitInventoryItem equipped = this.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);

                //{UnitStat[...]}
                //Replaced by values from the unit.Stats list
                MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
                if (unitStatMatches.Count > 0)
                {
                    foreach (Match match in unitStatMatches)
                    {
                        ModifiedStatValue unitStat;
                        if (!this.Stats.TryGetValue(match.Groups[1].Value, out unitStat))
                            throw new UnmatchedStatException(match.Groups[1].Value);
                        equation = equation.Replace(match.Groups[0].Value, unitStat.FinalValue.ToString());
                    }   
                }

                //{WeaponUtilStat}
                if (equation.Contains("{WeaponUtilStat}"))
                {
                    int weaponUtilStatValue = 0;
                    if(equipped != null)
                    {
                        foreach(string utilStatName in equipped.Item.UtilizedStats)
                        {
                            ModifiedStatValue weaponUtilStat;
                            if (!this.Stats.TryGetValue(utilStatName, out weaponUtilStat))
                                throw new UnmatchedStatException(utilStatName);

                            //Take the greatest stat value of all the utilized stats
                            if(weaponUtilStat.FinalValue > weaponUtilStatValue)
                                weaponUtilStatValue = weaponUtilStat.FinalValue;
                        }
                    }
                    equation = equation.Replace("{WeaponUtilStat}", weaponUtilStatValue.ToString()); 
                }

                //{WeaponStat[...]}
                MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
                if (weaponStatMatches.Count > 0)
                {
                    foreach (Match match in weaponStatMatches)
                    {
                        int weaponStatValue = 0;
                        if (equipped != null && !equipped.Item.Stats.TryGetValue(match.Groups[1].Value, out weaponStatValue))
                            throw new UnmatchedStatException(match.Groups[1].Value);
                        equation = equation.Replace(match.Groups[0].Value, weaponStatValue.ToString());
                    }
                }

                //Throw an error if anything remains unparsed
                if (equation.Contains("{") || equation.Contains("}"))
                    throw new UnrecognizedEquationVariableException(stat.Equation);

                Expression expression = new Expression(equation);
                this.CombatStats[stat.SourceName].BaseValue = Math.Max(0, Convert.ToInt32(expression.Evaluate()));
            }
        }
    
        public string GetUnitMovementType()
        {
            if (this.ClassList.Count > 0)
                return this.ClassList.First().MovementType;
            else return this.MovementType;
        }
    }
}
