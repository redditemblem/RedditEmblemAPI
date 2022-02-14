using NCalc;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Configuration.Units.CalculatedStats;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
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

        private static Regex unitStatRegex = new Regex(@"{UnitStat\[([A-Za-z]+)\]}"); //match unit stat name
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat\[([A-Za-z]+)\]}"); //match weapon stat name

        #endregion Constants

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitStatsData(UnitsConfig config, IList<string> data)
        {
            this.Level = ParseHelper.Int_NonZeroPositive(data, config.Level, "Level");

            int experience = ParseHelper.OptionalInt_Positive(data, config.Experience, "Experience", -1);
            if (experience > -1) experience %= 100;
            this.Experience = experience;

            this.HeldCurrency = ParseHelper.OptionalInt_Positive(data, config.HeldCurrency, "Currency");
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
        private void BuildCombatStats(IList<string> data, IList<CalculatedStatConfig> stats)
        {
            this.Combat = new Dictionary<string, ModifiedStatValue>();

            foreach (CalculatedStatConfig stat in stats)
            {
                ModifiedStatValue temp = new ModifiedStatValue();

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.Combat.Add(stat.SourceName, temp);
            }
        }

        private void BuildSystemStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            this.System = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.Int_Any(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
                    if (val == 0) continue;
                    temp.Modifiers.Add(mod.SourceName, val);
                }

                this.System.Add(stat.SourceName, temp);
            }
        }

        private void BuildGeneralStats(IList<string> data, IList<ModifiedNamedStatConfig> config)
        {
            this.General = new Dictionary<string, ModifiedStatValue>();

            foreach (ModifiedNamedStatConfig stat in config)
            {
                ModifiedStatValue temp = new ModifiedStatValue();
                temp.BaseValue = ParseHelper.Int_Positive(data, stat.BaseValue, stat.SourceName);

                //Parse modifiers list
                foreach (NamedStatConfig mod in stat.Modifiers)
                {
                    int val = ParseHelper.OptionalInt_Any(data, mod.Value, $"{stat.SourceName} {mod.SourceName}");
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
        public void CalculateCombatStats(IList<CalculatedStatConfig> stats, IList<UnitInventoryItem> unitInventory)
        {
            foreach (CalculatedStatConfig stat in stats)
            {
                string equation = stat.Equation;
                UnitInventoryItem equipped = unitInventory.SingleOrDefault(i => i != null && i.IsEquipped);

                //{UnitStat[...]}
                //Replaced by values from the Basic stat list
                MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
                if (unitStatMatches.Count > 0)
                {
                    foreach (Match match in unitStatMatches)
                    {
                        ModifiedStatValue unitStat;
                        if (!this.General.TryGetValue(match.Groups[1].Value, out unitStat))
                            throw new UnmatchedStatException(match.Groups[1].Value);
                        equation = equation.Replace(match.Groups[0].Value, unitStat.FinalValue.ToString());
                    }
                }

                //{WeaponUtilStat}
                if (equation.Contains("{WeaponUtilStat}"))
                {
                    int weaponUtilStatValue = 0;
                    if (equipped != null)
                    {
                        foreach (string utilStatName in equipped.Item.UtilizedStats)
                        {
                            ModifiedStatValue weaponUtilStat;
                            if (!this.General.TryGetValue(utilStatName, out weaponUtilStat))
                                throw new UnmatchedStatException(utilStatName);

                            //Take the greatest stat value of all the utilized stats
                            if (weaponUtilStat.FinalValue > weaponUtilStatValue)
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
                this.Combat[stat.SourceName].BaseValue = Math.Max(0, Convert.ToInt32(expression.Evaluate()));
            }
        }

    }
}
