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
    /// <summary>
    /// Container object for storing stat (raw numbers) about a unit.
    /// </summary>
    public class UnitStatsData
    {
        #region Attributes

        /// <summary>
        /// The unit's current level.
        /// </summary>
        public int Level { get; set; }

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
        /// Collection of the unit's calculated combat stats. (ex. Atk/Hit)
        /// </summary>
        public IDictionary<string, ModifiedStatValue> Combat { get; set; }

        /// <summary>
        /// Collection of the unit's system stats.
        /// </summary>
        public IDictionary<string, ModifiedStatValue> System { get; set; }

        /// <summary>
        /// Collection of the unit's stat values. (ex. Str/Mag)
        /// </summary>
        public IDictionary<string, ModifiedStatValue> General { get; set; }

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
            this.HP = new HP(data, config.HP.Current, config.HP.Maximum);

            //Build stat dictionaries
            this.Combat = BuildCombatStats(data, config.CombatStats);
            this.System = BuildModifiedStatDictionary(data, config.SystemStats, false);
            this.General = BuildModifiedStatDictionary(data, config.Stats);
        }

        #region Build Functions

        /// <summary>
        /// Iterates <paramref name="calculatedStats"/> and builds a dictionary of values. Does NOT calculate the stat's base value.
        /// </summary>
        private IDictionary<string, ModifiedStatValue> BuildCombatStats(IEnumerable<string> data, List<CalculatedStatConfig> calculatedStats)
        {
            IDictionary<string, ModifiedStatValue> stats = new Dictionary<string, ModifiedStatValue>();

            foreach (CalculatedStatConfig stat in calculatedStats)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.Modifiers = DataParser.NamedStatDictionary_OptionalInt_Any(stat.Modifiers, data, false, "{1} {0}", stat.SourceName);

                stats.Add(stat.SourceName, temp);
            }

            return stats;
        }

        /// <summary>
        /// Iterates through the stats in <paramref name="config"/> and builds a dictionary of values.
        /// </summary>
        private IDictionary<string, ModifiedStatValue> BuildModifiedStatDictionary(IEnumerable<string> data, List<ModifiedNamedStatConfig> config, bool requireBaseValue = true)
        {
            IDictionary<string, ModifiedStatValue> stats = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                
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
        public void CalculateCombatStats(List<CalculatedStatConfig> stats, Unit unit)
        {
            List<ReplaceCombatStatFormulaVariableEffect> replacementEffects = unit.GetFullSkillsList().SelectMany(s => s.Effects).OfType<ReplaceCombatStatFormulaVariableEffect>().ToList();
            string equippedUtilStat = GetItemUtilizedStatName(unit.Inventory.GetPrimaryEquippedItem());

            foreach (CalculatedStatConfig stat in stats)
            {
                //Select the correct equation
                string equation = stat.Equations.First().Equation;
                if (stat.SelectsUsing == CalculatedStatEquationSelectorEnum.EquippedWeaponUtilizedStat && !string.IsNullOrEmpty(equippedUtilStat))
                {
                    CalculatedStatEquationConfig equationConfig = stat.Equations.FirstOrDefault(e => e.SelectValue == equippedUtilStat);
                    if (equationConfig != null)
                        equation = equationConfig.Equation;
                }

                //Check if skill effects replace any formula variables
                foreach (ReplaceCombatStatFormulaVariableEffect effect in replacementEffects.Where(re => re.Stats.Contains(stat.SourceName)))
                {
                    for (int i = 0; i < effect.VariablesToReplace.Count; i++)
                        equation = equation.Replace(effect.VariablesToReplace[i], effect.VariablesToUse[i]);
                }

                //Evaluate and round the result
                EquationParserOptions options = new EquationParserOptions()
                {
                    EvalUnitCombatStat = true,
                    EvalUnitStat = true,
                    EvalUnitLevel = true,
                    EvalWeaponUtilStat = true,
                    EvalWeaponStat = true,
                    EvalBattalionStat = true
                };

                decimal equationResult = EquationParser.Evaluate(equation, unit, options);
                this.Combat[stat.SourceName].BaseValue = Math.Max(0, Convert.ToInt32(Math.Floor(equationResult)));
            }
        }

        private string GetItemUtilizedStatName(UnitInventoryItem item)
        {
            string statName = string.Empty;
            int maxValue = int.MinValue;

            if (item == null) return statName;

            foreach (string utilStatName in item.Item.UtilizedStats)
            {
                ModifiedStatValue weaponUtilStat = MatchGeneralStatName(utilStatName);

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
        public ModifiedStatValue MatchCombatStatName(string name)
        {
            ModifiedStatValue stat;
            if (!this.Combat.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        /// <summary>
        /// Returns the stat in <c>System</c> that matches <paramref name="name"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public ModifiedStatValue MatchSystemStatName(string name)
        {
            ModifiedStatValue stat;
            if (!this.System.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }

        /// <summary>
        /// Returns the stat in <c>General</c> that matches <paramref name="name"/>.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        public ModifiedStatValue MatchGeneralStatName(string name)
        {
            ModifiedStatValue stat;
            if (!this.General.TryGetValue(name, out stat))
                throw new UnmatchedStatException(name);

            return stat;
        }
    }
}
