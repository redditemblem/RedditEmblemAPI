using NCalc;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System.Skills.Effects.UnitStats;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        #region Constants

        private static Regex unitStatRegex = new Regex(@"{UnitStat\[([A-Za-z,]+)\]}"); //match unit stat name
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat\[([A-Za-z,]+)\]}"); //match weapon stat name

        #endregion Constants

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitStatsData(UnitsConfig config, List<string> data)
        {
            this.Level = DataParser.Int_NonZeroPositive(data, config.Level, "Level");

            int experience = DataParser.OptionalInt_Positive(data, config.Experience, "Experience", -1);
            if (experience > -1) experience %= 100;
            this.Experience = experience;

            this.HeldCurrency = DataParser.OptionalInt_Positive(data, config.HeldCurrency, "Currency");
            this.HP = new HP(data, config.HP.Current, config.HP.Maximum);

            //Build stat dictionaries
            BuildCombatStats(data, config.CombatStats);
            BuildSystemStats(data, config.SystemStats);
            BuildGeneralStats(data, config.Stats);
        }

        #region Build Functions

        /// <summary>
        /// Adds the stats from <paramref name="stats"/> into <c>CombatStats</c>. Does NOT calculate their values.
        /// </summary>
        private void BuildCombatStats(List<string> data, List<CalculatedStatConfig> stats)
        {
            this.Combat = new Dictionary<string, ModifiedStatValue>();

            foreach (CalculatedStatConfig stat in stats)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = DataParser.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.Combat.Add(stat.SourceName, temp);
            }
        }

        private void BuildSystemStats(List<string> data, List<ModifiedNamedStatConfig> config)
        {
            this.System = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = DataParser.Int_Any(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = DataParser.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.System.Add(stat.SourceName, temp);
            }
        }

        private void BuildGeneralStats(List<string> data, List<ModifiedNamedStatConfig> config)
        {
            this.General = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = DataParser.Int_Any(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = DataParser.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.General.Add(stat.SourceName, temp);
            }
        }

        #endregion

        /// <summary>
        /// Assembles and executes the equations in <paramref name="stats"/>.
        /// </summary>
        public void CalculateCombatStats(List<CalculatedStatConfig> stats, List<UnitInventoryItem> unitInventory, List<ReplaceCombatStatFormulaVariableEffect> replacementEffects)
        {
            foreach (CalculatedStatConfig stat in stats)
            {
                UnitInventoryItem equipped = unitInventory.SingleOrDefault(i => i != null && i.IsPrimaryEquipped);
                string equippedUtilStat = GetItemUtilizedStatName(equipped);

                //Select the correct equation
                string equation = stat.Equations.First().Equation;
                if (stat.SelectsUsing == CalculatedStatEquationSelectorEnum.EquippedWeaponUtilizedStat && !string.IsNullOrEmpty(equippedUtilStat))
                {
                    CalculatedStatEquationConfig equationConfig = stat.Equations.FirstOrDefault(e => e.SelectValue == equippedUtilStat);
                    if (equationConfig != null)
                        equation = equationConfig.Equation;
                }


                //First, check if skill effects replace any formula variables
                foreach(ReplaceCombatStatFormulaVariableEffect effect in replacementEffects.Where(re => re.Stats.Contains(stat.SourceName)))
                {
                    for (int i = 0; i < effect.VariablesToReplace.Count; i++)
                        equation = equation.Replace(effect.VariablesToReplace[i], effect.VariablesToUse[i]);
                }

                //{UnitStat[...]}
                //Replaced by values from the General stat list
                MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
                if (unitStatMatches.Count > 0)
                {
                    foreach (Match match in unitStatMatches)
                    {
                        int maximumStatValue = int.MinValue;
                        foreach(string statSplit in match.Groups[1].Value.Split(","))
                        {
                            ModifiedStatValue unitStat;
                            if (!this.General.TryGetValue(statSplit, out unitStat))
                                throw new UnmatchedStatException(statSplit);

                            if(maximumStatValue < unitStat.FinalValue)
                                maximumStatValue = unitStat.FinalValue;
                        }
                        
                        equation = equation.Replace(match.Groups[0].Value, maximumStatValue.ToString());
                    }
                }

                //{UnitLevel}
                if (equation.Contains("{UnitLevel}"))
                {
                    equation = equation.Replace("{UnitLevel}", this.Level.ToString());
                }

                //{WeaponUtilStat}
                if (equation.Contains("{WeaponUtilStat}"))
                {
                    int value = 0;
                    if (!string.IsNullOrEmpty(equippedUtilStat))
                        value = this.General[equippedUtilStat].FinalValue;

                    equation = equation.Replace("{WeaponUtilStat}", value.ToString());
                }

                //{WeaponStat[...]}
                MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
                if (weaponStatMatches.Count > 0)
                {
                    foreach (Match match in weaponStatMatches)
                    {
                        int maximumStatValue = int.MinValue;

                        if(equipped != null)
                        {
                            foreach (string statSplit in match.Groups[1].Value.Split(","))
                            {
                                int statValue;
                                if (!equipped.Item.Stats.TryGetValue(statSplit, out statValue))
                                    throw new UnmatchedStatException(statSplit);

                                if (maximumStatValue < statValue)
                                    maximumStatValue = statValue;
                            }
                        }
                        else
                        {
                            //If there is no equipped item, default value to 0.
                            maximumStatValue = 0;
                        }

                        equation = equation.Replace(match.Groups[0].Value, maximumStatValue.ToString());
                    }
                }

                //Throw an error if anything remains unparsed
                if (equation.Contains("{") || equation.Contains("}"))
                    throw new UnrecognizedEquationVariableException(equation);

                try
                {
                    Expression expression = new Expression(equation);
                    this.Combat[stat.SourceName].BaseValue = Math.Max(0, Convert.ToInt32(Math.Floor(Convert.ToDecimal(expression.Evaluate()))));
                }
                catch(Exception ex)
                {
                    throw new EquationEvaluationErrorException(equation);
                }
            }
        }

    
        private string GetItemUtilizedStatName(UnitInventoryItem item)
        {
            string statName = string.Empty;
            int maxValue = int.MinValue;

            if (item == null)
                return statName;

            foreach (string utilStatName in item.Item.UtilizedStats)
            {
                ModifiedStatValue weaponUtilStat;
                if (!this.General.TryGetValue(utilStatName, out weaponUtilStat))
                    throw new UnmatchedStatException(utilStatName);

                //Take the greatest stat value of all the utilized stats
                if (weaponUtilStat.FinalValue > maxValue)
                {
                    statName = utilStatName;
                    maxValue = weaponUtilStat.FinalValue;
                }
            }

            return statName;
        }
    }
}
