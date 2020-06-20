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
            this.Name = ParseHelper.SafeStringParse(data, config.Name, "Name", true);
            this.SpriteURL = ParseHelper.SafeStringParse(data, config.SpriteURL, "Sprite URL", true);
            this.Player = ParseHelper.SafeStringParse(data, config.Player, "Player", false);
            this.Coordinate = new Coordinate(data.ElementAtOrDefault<string>(config.Coordinate));
            this.UnitSize = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.UnitSize), "Unit Size", true, 1);
            this.HasMoved = (ParseHelper.SafeStringParse(data, config.HasMoved, "Has Moved", false) == "Yes");
            this.HP = new HP(data.ElementAtOrDefault<string>(config.HP.Current),
                             data.ElementAtOrDefault<string>(config.HP.Maximum));
            this.Level = ParseHelper.SafeIntParse(data.ElementAtOrDefault(config.Level), "Level", true);
            this.Experience = ParseHelper.SafeIntParse(data, config.Experience, "Experience", true) % 100;
            this.HeldCurrency = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.HeldCurrency), "Currency", false, -1);
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
            Affiliation affMatch;
            if (!systemData.Affiliations.TryGetValue(data.ElementAtOrDefault<string>(config.Affiliation), out affMatch))
                throw new UnmatchedAffiliationException(data.ElementAtOrDefault<string>(config.Affiliation));
            this.AffiliationObj = affMatch;
            affMatch.Matched = true;

            this.WeaponRanks = new Dictionary<string, string>();
            BuildWeaponRanks(data, config.WeaponRanks);

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
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault<string>(rank.Type)))
                    this.WeaponRanks.Add(data.ElementAtOrDefault<string>(rank.Type), data.ElementAtOrDefault<string>(rank.Rank) ?? string.Empty);
        }

        private void BuildStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(stat.BaseValue), stat.SourceName, true);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(mod.Value), string.Format("{0} {1}", stat.SourceName, mod.SourceName), false);
                    if (val != 0)
                        temp.Modifiers.Add(mod.SourceName, val);
                }

                this.Stats.Add(stat.SourceName, temp);
            }
        }

        private void BuildStatusConditions(IList<string> data, IList<int> config, IDictionary<string, StatusCondition> statuses)
        {
            foreach(int field in config)
            {
                //Skip blank cells
                string name = data.ElementAtOrDefault<string>(field);
                if (string.IsNullOrEmpty(name))
                    continue;

                UnitStatus status = new UnitStatus(name, statuses);
                this.StatusConditions.Add(status);
            }
        }

        private void BuildInventory(IList<string> data, InventoryConfig config, IDictionary<string, Item> items, IList<string> weaponRanks)
        {
            foreach (int field in config.Slots)
            {
                string name = data.ElementAtOrDefault(field);
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
                else if (string.IsNullOrEmpty(item.Item.WeaponRank) && string.IsNullOrEmpty(item.Item.UtilizedStat) && !item.Item.DealsDamage)
                {
                    item.CanEquip = true;
                }

                this.Inventory.Add(item);
            }

            //Find the equipped item and flag it
            if (!string.IsNullOrEmpty(data.ElementAtOrDefault(config.EquippedItem)))
            {
                UnitInventoryItem equipped = this.Inventory.FirstOrDefault(i => i.FullName == data.ElementAt(config.EquippedItem));
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(data.ElementAt(config.EquippedItem));
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
    
        private void BuildSkills(IList<string> data, IList<int> fields, IDictionary<string, Skill> skills)
        {
            foreach (int field in fields)
            {
                //Skip blank cells
                string name = data.ElementAtOrDefault<string>(field).Trim();
                if (string.IsNullOrEmpty(name))
                    continue;

                Skill match;
                if (!skills.TryGetValue(name, out match))
                    throw new UnmatchedSkillException(name);
                match.Matched = true;

                this.SkillList.Add(match);
            }
        }
    
        private void BuildClasses(IList<string> data, IList<int> config, IDictionary<string, Class> classes)
        {
            foreach (int field in config)
            {
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault(field)))
                {
                    Class match;
                    if (!classes.TryGetValue(data.ElementAt(field), out match))
                        throw new UnmatchedClassException(data.ElementAt(field));

                    match.Matched = true;
                    this.ClassList.Add(match);

                    //Append class tags to unit's tags
                    this.Tags = this.Tags.Union(match.Tags).Distinct().ToList();
                }
            }

            if (this.ClassList.Count < 1)
                throw new Exception("Unit must have at least one class defined.");
        }

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

                //{UnitStat[...]}
                //Replaced by values from the unit.Stats list
                MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
                if (unitStatMatches.Count > 0)
                {
                    foreach (Match match in unitStatMatches)
                        equation = equation.Substring(0, match.Index) + this.Stats[match.Groups[1].Value].FinalValue + equation.Substring(match.Index + match.Length);
                }

                //{WeaponUtilStat}
                UnitInventoryItem equipped = this.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
                equation = equation.Replace("{WeaponUtilStat}", (equipped != null ? this.Stats[equipped.Item.UtilizedStat].FinalValue : 0).ToString());

                //{WeaponStat[...]}
                MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
                if (weaponStatMatches.Count > 0)
                {
                    foreach (Match match in weaponStatMatches)
                        equation = equation.Substring(0, match.Index) + (equipped != null ? equipped.Item.Stats[match.Groups[1].Value] : 0) + equation.Substring(match.Index + match.Length);
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
