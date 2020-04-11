﻿using NCalc;
using Newtonsoft.Json;
using RedditEmblemAPI.Models.Common;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output
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
        public Coordinate Coordinates { get; set; }

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
        public string Affiliation { get; set; }

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
        public Dictionary<string, int> CalculatedStats { get; set; }

        /// <summary>
        /// Collection of the unit's stat values.
        /// </summary>
        public Dictionary<string, ModifiedStatValue> Stats { get; set; }

        /// <summary>
        /// List of the items the unit is carrying.
        /// </summary>
        public IList<UnitHeldItem> Inventory { get; set; }

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
        public Unit(UnitsConfig config, IList<string> data, IDictionary<string, Class> classes, IDictionary<string, Item> items, IDictionary<string, Skill> skills)
        {
            this.Name = data.ElementAtOrDefault<string>(config.UnitName).Trim();
            this.SpriteURL = data.ElementAtOrDefault<string>(config.SpriteURL);
            this.Coordinates = new Coordinate(data.ElementAtOrDefault<string>(config.Coordinates));
            this.UnitSize = ParseHelper.OptionalSafeIntParse(data.ElementAtOrDefault<string>(config.UnitSize), "Unit Size", true, 1);
            this.HasMoved = (data.ElementAtOrDefault<string>(config.HasMoved) == "Yes");
            this.HP = new HP(data.ElementAtOrDefault<string>(config.CurrentHP),
                             data.ElementAtOrDefault<string>(config.MaxHP));
            this.Level = ParseHelper.SafeIntParse(data.ElementAtOrDefault(config.Level), "Level", true);
            this.Affiliation = data.ElementAtOrDefault<string>(config.Affiliation);
            this.Experience = ParseHelper.SafeIntParse(data.ElementAtOrDefault<string>(config.Experience), "Experience", true) % 100;
            this.HeldCurrency = ParseHelper.SafeIntParse(data.ElementAtOrDefault(config.HeldCurrency) ?? "-1", "Currency", false);
            this.TextFields = ParseHelper.StringListParse(data, config.TextFields);
            this.Tags = ParseHelper.StringCSVParse(data, config.Tags);

            this.MovementRange = new List<Coordinate>();
            this.AttackRange = new List<Coordinate>();
            this.UtilityRange = new List<Coordinate>();

            //Find unit number
            Match numberMatch = unitNumberRegex.Match(this.Name);
            if (numberMatch.Success)
                this.UnitNumber = numberMatch.Value.Trim();

            this.Stats = new Dictionary<string, ModifiedStatValue>();
            foreach (ModifiedNamedStatConfig s in config.Stats)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse base value
                int val;
                if (!int.TryParse(data.ElementAtOrDefault(s.BaseValue), out val) || val < 0)
                    throw new PositiveIntegerException(s.SourceName, data.ElementAtOrDefault<string>(s.BaseValue));
                temp.BaseValue = val;

                //Parse modifiers list
                foreach (NamedStatConfig mod in s.Modifiers)
                {
                    if (!int.TryParse(data.ElementAtOrDefault(mod.Value), out val))
                        throw new AnyIntegerException(string.Format("{0} {1}", s.SourceName, mod.SourceName), data.ElementAtOrDefault<string>(mod.Value));

                    if (val != 0)
                        temp.Modifiers.Add(mod.SourceName, val);
                }

                this.Stats.Add(s.SourceName, temp);
            }

            this.Inventory = new List<UnitHeldItem>();
            BuildInventory(data, config.Inventory, items);

            this.SkillList = new List<Skill>();
            BuildSkills(data, config.Skills, skills);

            this.ClassList = new List<Class>();
            BuildClasses(data, config.Classes, classes);

            //This needs to be run last
            this.CalculatedStats = new Dictionary<string, int>();
            CalculateCombatStats(config.CalculatedStats);
        }

        private void BuildInventory(IList<string> data, InventoryConfig config, IDictionary<string, Item> items)
        {
            foreach (int field in config.Slots)
            {
                string Name = data.ElementAtOrDefault(field);
                if (string.IsNullOrEmpty(Name))
                {
                    this.Inventory.Add(null);
                    continue;
                }

                this.Inventory.Add(new UnitHeldItem(Name, items));
            }

            //Find the equipped item and flag it
            if (!string.IsNullOrEmpty(data.ElementAtOrDefault(config.EquippedItem)))
            {
                UnitHeldItem equipped = this.Inventory.FirstOrDefault(i => i.FullName == data.ElementAt(config.EquippedItem));
                if (equipped == null)
                    throw new UnmatchedEquippedItemException(data.ElementAt(config.EquippedItem));
                equipped.IsEquipped = true;

                //Apply equipped stat modifiers
                foreach (string stat in equipped.Item.EquippedStatModifiers.Keys)
                {
                    ModifiedStatValue mods = this.Stats.GetValueOrDefault(stat);
                    if (mods != null)
                        mods.Modifiers.Add(string.Format("{0} ({1})", equipped.FullName, "Eqp"), equipped.Item.EquippedStatModifiers[stat]);
                }
            }

            //Apply inventory stat modifiers for all nonequipped items
            foreach (UnitHeldItem inv in this.Inventory.Where(i => i != null && !i.IsEquipped))
            {
                foreach (string stat in inv.Item.InventoryStatModifiers.Keys)
                {
                    ModifiedStatValue mods = this.Stats.GetValueOrDefault(stat);
                    if (mods != null)
                        mods.Modifiers.Add(string.Format("{0} ({1})", inv.Item.Name, "Inv"), inv.Item.InventoryStatModifiers[stat]);
                }
            }

        }
    
        private void BuildSkills(IList<string> data, SkillListConfig config, IDictionary<string, Skill> skills)
        {
            foreach (int slot in config.Slots)
            {
                //Skip blank cells
                string name = data.ElementAtOrDefault<string>(slot).Trim();
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
        }
    
        private void CalculateCombatStats(IList<CalculatedStatConfig> stats)
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
                UnitHeldItem equipped = this.Inventory.SingleOrDefault(i => i != null && i.IsEquipped);
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
                this.CalculatedStats.Add(stat.SourceName, (int)expression.Evaluate());
            }
        }
    }
}
