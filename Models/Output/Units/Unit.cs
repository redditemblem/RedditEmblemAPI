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
        /// List of the unit's text fields.
        /// </summary>
        public IList<string> TextFields { get; set; }

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
        public Tile OriginTile { get; set; }

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
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat[A-Za-z]+]}"); //match weapon stat name

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public Unit(UnitsConfig config, IList<string> data, SystemInfo systemData)
        {
            //Required fields
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", true);
            this.Level = ParseHelper.SafeIntParse(data, config.Level, "Level", true);
            this.Coordinate = new Coordinate(data.ElementAtOrDefault<string>(config.Coordinate));
            this.HP = new HP(data.ElementAtOrDefault<string>(config.HP.Current),
                             data.ElementAtOrDefault<string>(config.HP.Maximum));

            //Optional fields
            this.Player = ParseHelper.SafeStringParse(data, config.Player, "Player", false);
            this.UnitSize = ParseHelper.OptionalSafeIntParse(data, config.UnitSize, "Unit Size", true, 1);
            this.HasMoved = (ParseHelper.SafeStringParse(data, config.HasMoved, "Has Moved", false) == "Yes");
            this.Experience = ParseHelper.OptionalSafeIntParse(data, config.Experience, "Experience", true, 0) % 100;
            this.HeldCurrency = ParseHelper.OptionalSafeIntParse(data, config.HeldCurrency, "Currency", true, -1);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
            this.Tags = ParseHelper.StringCSVParse(data, config.Tags);

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

            this.WeaponRanks = new Dictionary<string, string>();
            BuildWeaponRanks(data, config.WeaponRanks);

            this.SystemStats = new Dictionary<string, ModifiedStatValue>();
            BuildSystemStats(data, config.SystemStats);

            this.Stats = new Dictionary<string, ModifiedStatValue>();
            BuildStats(data, config.Stats);

            this.StatusConditions = new List<UnitStatus>();
            BuildStatusConditions(data, config.StatusConditions, systemData.StatusConditions);

            this.Inventory = new List<UnitInventoryItem>();
            BuildInventory(data, config.Inventory, systemData.Items, systemData.WeaponRanks);

            this.SkillList = new List<Skill>();
            BuildSkills(data, config.Skills, systemData.Skills);

            this.ClassList = new List<Class>();
            BuildClasses(data, config.Classes, systemData.Classes);

            this.CombatStats = new Dictionary<string, ModifiedStatValue>();
            BuildCombatStats(config.CombatStats);
        }

        private void BuildWeaponRanks(IList<string> data, IList<UnitWeaponRanksConfig> config)
        {
            foreach (UnitWeaponRanksConfig rank in config)
            {
                string rankType = ParseHelper.SafeStringParse(data, rank.Type, "Weapon Rank Type", false);
                string rankLetter = ParseHelper.SafeStringParse(data, rank.Rank, "Weapon Rank Letter", false);

                if (!string.IsNullOrEmpty(rankType))
                    this.WeaponRanks.Add(rankType, rankLetter);
            }
        }

        private void BuildSystemStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            foreach(ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.SafeIntParse(data, stat.BaseValue, stat.SourceName, true);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.SafeIntParse(data, mod.Value, string.Format("{0} {1}", stat.SourceName, mod.SourceName), false);
                    if (val == 0)
                        continue;
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
                temp.BaseValue = ParseHelper.SafeIntParse(data, stat.BaseValue, stat.SourceName, true);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.SafeIntParse(data, mod.Value, string.Format("{0} {1}", stat.SourceName, mod.SourceName), false);
                    if (val == 0)
                        continue;
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

        private void BuildInventory(IList<string> data, InventoryConfig config, IDictionary<string, Item> items, IList<string> weaponRanks)
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
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && string.IsNullOrEmpty(item.Item.UtilizedStat))
                {
                    item.CanEquip = true;
                }

                this.Inventory.Add(item);
            }

            //Find the equipped item and flag it
            string equippedItemName = ParseHelper.SafeStringParse(data, config.EquippedItem, "Equipped Item", false);
            if (!string.IsNullOrEmpty(equippedItemName))
            {
                UnitInventoryItem equipped = this.Inventory.FirstOrDefault(i => i.FullName == equippedItemName);
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

            if (this.ClassList.Count < 1)
                throw new Exception("Unit must have at least one class defined.");
        }

        /// <summary>
        /// Adds the stats from <paramref name="stats"/> into <c>CombatStats</c>. Does NOT calculate their values.
        /// </summary>
        private void BuildCombatStats(IList<CalculatedStatConfig> stats)
        {
            foreach (CalculatedStatConfig stat in stats)
            {
                this.CombatStats.Add(stat.SourceName, new ModifiedStatValue());
            }
        }

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
                        equation = equation.Substring(0, match.Index) + unitStat.FinalValue + equation.Substring(match.Index + match.Length);
                    }   
                }

                //{WeaponUtilStat}
                if (equation.Contains("{WeaponUtilStat}"))
                {
                    ModifiedStatValue weaponUtilStat;
                    if(!this.Stats.TryGetValue(equipped.Item.UtilizedStat, out weaponUtilStat) && !string.IsNullOrEmpty(equipped.Item.UtilizedStat))
                        throw new UnmatchedStatException(equipped.Item.UtilizedStat);
                    equation = equation.Replace("{WeaponUtilStat}", (equipped != null && weaponUtilStat != null ? weaponUtilStat.FinalValue : 0).ToString()); 
                }

                //{WeaponStat[...]}
                MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
                if (weaponStatMatches.Count > 0)
                {
                    foreach (Match match in weaponStatMatches)
                    {
                        int weaponStatValue = 0;
                        if (equipped != null && equipped.Item.Stats.TryGetValue(match.Groups[1].Value, out weaponStatValue))
                            throw new UnmatchedStatException(match.Groups[1].Value);
                        equation = equation.Substring(0, match.Index) + weaponStatValue + equation.Substring(match.Index + match.Length);
                    }
                }

                //Throw an error if anything remains unparsed
                if (equation.Contains("{") || equation.Contains("}"))
                    throw new UnrecognizedEquationVariableException(stat.Equation);

                Expression expression = new Expression(equation);
                this.CombatStats[stat.SourceName].BaseValue = (int)expression.Evaluate();
            }
        }
    }
}
