using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Units
{
    #region Interface

    /// <inheritdoc cref="UnitStatsData"/>
    public interface IUnitStatsData
    {
        /// <inheritdoc cref="UnitStatsData.Level"/>
        int Level { get; }

        /// <inheritdoc cref="UnitStatsData.Experience"/>
        int Experience { get; }

        /// <inheritdoc cref="UnitStatsData.HeldCurrency"/>
        int HeldCurrency { get; }

        /// <inheritdoc cref="UnitStatsData.HP"/>
        IHealthPoints HP { get; }

        /// <inheritdoc cref="UnitStatsData.Combat"/>
        IDictionary<string, IModifiedStatValue> Combat { get; }

        /// <inheritdoc cref="UnitStatsData.System"/>
        IDictionary<string, IModifiedStatValue> System { get; }

        /// <inheritdoc cref="UnitStatsData.General"/>
        IDictionary<string, IModifiedStatValue> General { get; }

        /// <inheritdoc cref="UnitStatsData.CalculateCombatStats(List{CalculatedStatConfig}, IUnit)"/>
        void CalculateCombatStats(List<CalculatedStatConfig> stats, IUnit unit);

        /// <inheritdoc cref="UnitStatsData.MatchCombatStatName(string)"/>
        IModifiedStatValue MatchCombatStatName(string name);

        /// <inheritdoc cref="UnitStatsData.ApplyCombatStatModifiers(IDictionary{string, int}, string, bool)"/>
        void ApplyCombatStatModifiers(IDictionary<string, int> modifiers, string modifierName, bool allowModifiersToStack = false);

        /// <inheritdoc cref="UnitStatsData.MatchSystemStatName(string)"/>
        IModifiedStatValue MatchSystemStatName(string name);

        /// <inheritdoc cref="UnitStatsData.MatchGeneralStatName(string)"/>
        IModifiedStatValue MatchGeneralStatName(string name);

        /// <inheritdoc cref="UnitStatsData.ApplyGeneralStatModifiers(IDictionary{string, int}, string, bool)"/>
        void ApplyGeneralStatModifiers(IDictionary<string, int> modifiers, string modifierName, bool allowModifiersToStack = false);
    }

    #endregion Interface

    /// <summary>
    /// Container object for storing stat (raw numbers) about a unit.
    /// </summary>
    public class UnitStatsData : IUnitStatsData
    {
        #region Attributes

        /// <summary>
        /// The unit's current level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// The unit's earned experience.
        /// </summary>
        public int Experience { get; private set; }

        /// <summary>
        /// The amount of currency the unit has in their wallet.
        /// </summary>
        public int HeldCurrency { get; private set; }

        /// <summary>
        /// Container object for HP values.
        /// </summary>
        public IHealthPoints HP { get; private set; }

        /// <summary>
        /// Collection of the unit's calculated combat stats. (ex. Atk/Hit)
        /// </summary>
        public IDictionary<string, IModifiedStatValue> Combat { get; private set; }

        /// <summary>
        /// Collection of the unit's system stats.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, IModifiedStatValue> System { get; private set; }

        /// <summary>
        /// Collection of the unit's stat values. (ex. Str/Mag)
        /// </summary>
        public IDictionary<string, IModifiedStatValue> General { get; private set; }

        #region JSON Serialization ONLY

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private IDictionary<string, IModifiedStatValue> System_Prioritized 
        { 
            get
            {
                var prioritized = this.System.Where(s => s.Value.UsePrioritizedDisplay);
                return prioritized.Count() == 0 ? null : prioritized.ToDictionary(); 
            } 
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private IDictionary<string, IModifiedStatValue> System_NonPrioritized
        {
            get
            {
                var nonPrioritized = this.System.Where(s => !s.Value.UsePrioritizedDisplay);
                return nonPrioritized.Count() == 0 ? null : nonPrioritized.ToDictionary();
            }
        }

        #endregion JSON Serialization ONLY

        #endregion Attributes

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitStatsData(UnitsConfig config, IEnumerable<string> data)
        {
            this.Level = DataParser.Int_NonZeroPositive(data, config.Level, "Level");

            int experience = DataParser.OptionalInt_Positive(data, config.Experience, "Experience", -1);
            if (experience > -1) experience %= 100;
            this.Experience = experience;

            this.HeldCurrency = DataParser.OptionalInt_Positive(data, config.HeldCurrency, "Currency");
            this.HP = new HealthPoints(data, config.HP);

            //Build stat dictionaries
            this.Combat = BuildCombatStats(data, config.CombatStats);
            this.System = BuildModifiedStatDictionary(data, config.SystemStats, false);
            this.General = BuildModifiedStatDictionary(data, config.Stats);
        }

        #region Build Functions

        /// <summary>
        /// Iterates <paramref name="calculatedStats"/> and builds a dictionary of values. Does NOT calculate the stat's base value.
        /// </summary>
        private IDictionary<string, IModifiedStatValue> BuildCombatStats(IEnumerable<string> data, List<CalculatedStatConfig> calculatedStats)
        {
            IDictionary<string, IModifiedStatValue> stats = new Dictionary<string, IModifiedStatValue>();

            foreach (CalculatedStatConfig stat in calculatedStats)
            {
                IModifiedStatValue temp = new ModifiedStatValue(stat.InvertModifiedDisplayColors, false);
                temp.Modifiers = DataParser.NamedStatDictionary_OptionalInt_Any(stat.Modifiers, data, false, "{1} {0}", stat.SourceName);

                stats.Add(stat.SourceName, temp);
            }

            return stats;
        }

        /// <summary>
        /// Iterates through the stats in <paramref name="config"/> and builds a dictionary of values.
        /// </summary>
        private IDictionary<string, IModifiedStatValue> BuildModifiedStatDictionary(IEnumerable<string> data, List<ModifiedNamedStatConfig_Displayed> config, bool requireBaseValue = true)
        {
            IDictionary<string, IModifiedStatValue> stats = new Dictionary<string, IModifiedStatValue>();

            foreach (ModifiedNamedStatConfig_Displayed stat in config)
            {
                IModifiedStatValue temp = new ModifiedStatValue(stat.InvertModifiedDisplayColors, stat.UsePrioritizedDisplay);
                
                if(requireBaseValue) temp.BaseValue = DataParser.Int_Any(data, stat.BaseValue, stat.SourceName);
                else temp.BaseValue = DataParser.OptionalInt_Any(data, stat.BaseValue, stat.SourceName);

                temp.Modifiers = DataParser.NamedStatDictionary_OptionalInt_Any(stat.Modifiers, data, false, "{1} {0}", stat.SourceName);

                stats.Add(stat.SourceName, temp);
            }

            return stats;
        }

        #endregion

        /// <summary>
        /// Assembles and executes the equations in <paramref name="stats"/>.
        /// </summary>
        public void CalculateCombatStats(List<CalculatedStatConfig> stats, IUnit unit)
        {
            List<ReplaceCombatStatFormulaVariableEffect> replacementEffects = unit.GetFullSkillsList().SelectMany(s => s.Effects).OfType<ReplaceCombatStatFormulaVariableEffect>().ToList();
            string equippedUtilStat = GetItemUtilizedStatName(unit.Inventory.GetPrimaryEquippedItem());

            foreach (CalculatedStatConfig stat in stats)
            {
                //Default to the first equation in the list
                CalculatedStatEquationConfig equationConfig = stat.Equations.First();

                //If needed, execute on the SelectsUsing option
                if (stat.SelectsUsing == CalculatedStatEquationSelectorEnum.EquippedWeaponUtilizedStat && !string.IsNullOrEmpty(equippedUtilStat))
                {
                    CalculatedStatEquationConfig selectsUsingUtilStat = stat.Equations.FirstOrDefault(e => e.SelectValue == equippedUtilStat);
                    if (selectsUsingUtilStat != null)
                        equationConfig = selectsUsingUtilStat;
                }

                //Select equation and parser options to use
                string equation = equationConfig.Equation;
                EquationParserOptions options = equationConfig.ParserOptions;

                //Check if skill effects replace any formula variables
                foreach (ReplaceCombatStatFormulaVariableEffect effect in replacementEffects.Where(re => re.Stats.Contains(stat.SourceName)))
                {
                    for (int i = 0; i < effect.VariablesToReplace.Count; i++)
                        equation = equation.Replace(effect.VariablesToReplace[i], effect.VariablesToUse[i]);
                    options = options.Union(effect.ParserOptions);
                }

                decimal equationResult = EquationParser.Evaluate(equation, unit, options);
                this.Combat[stat.SourceName].BaseValue = Math.Max(0, Convert.ToInt32(Math.Floor(equationResult)));
            }
        }

        private string GetItemUtilizedStatName(IUnitInventoryItem item)
        {
            string statName = string.Empty;
            int maxValue = int.MinValue;

            if (item == null) return statName;

            foreach (string utilStatName in item.Item.UtilizedStats)
            {
                IModifiedStatValue weaponUtilStat = MatchGeneralStatName(utilStatName);

                //Take the greatest stat value of all the utilized stats
                if (weaponUtilStat.FinalValue > maxValue)
                {
                    statName = utilStatName;
                    maxValue = weaponUtilStat.FinalValue;
                }
            }

            return statName;
        }

        /// <summary>
        /// Returns the stat in <c>Combat</c> that matches <paramref name="name"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public IModifiedStatValue MatchCombatStatName(string name)
        {
            IModifiedStatValue stat;
            if (!this.Combat.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        /// <summary>
        /// Applies each modifier in <paramref name="modifiers"/> to the matching combat stat.
        /// </summary>
        /// <param name="allowModifiersToStack">If true, will add values to any existing modifier with the same <paramref name="modifierName"/>.</param>
        public void ApplyCombatStatModifiers(IDictionary<string, int> modifiers, string modifierName, bool allowModifiersToStack = false)
        {
            foreach (KeyValuePair<string, int> mod in modifiers)
            {
                IModifiedStatValue stat = this.MatchCombatStatName(mod.Key);
                if (!stat.Modifiers.TryAdd(modifierName, mod.Value) && allowModifiersToStack)
                    stat.Modifiers[modifierName] += mod.Value;
            }
        }

        /// <summary>
        /// Returns the stat in <c>System</c> that matches <paramref name="name"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public IModifiedStatValue MatchSystemStatName(string name)
        {
            IModifiedStatValue stat;
            if (!this.System.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        /// <summary>
        /// Returns the stat in <c>General</c> that matches <paramref name="name"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public IModifiedStatValue MatchGeneralStatName(string name)
        {
            IModifiedStatValue stat;
            if (!this.General.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        /// <summary>
        /// Applies each modifier in <paramref name="modifiers"/> to the matching general stat.
        /// </summary>
        /// <param name="allowModifiersToStack">If true, will add values to any existing modifier with the same <paramref name="modifierName"/>.</param>
        public void ApplyGeneralStatModifiers(IDictionary<string, int> modifiers, string modifierName, bool allowModifiersToStack = false)
        {
            foreach (KeyValuePair<string, int> mod in modifiers)
            {
                IModifiedStatValue stat = this.MatchGeneralStatName(mod.Key);
                if (!stat.Modifiers.TryAdd(modifierName, mod.Value) && allowModifiersToStack)
                    stat.Modifiers[modifierName] += mod.Value;
            }
        }

    }
}
