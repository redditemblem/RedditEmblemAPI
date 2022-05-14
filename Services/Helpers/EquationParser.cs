using NCalc;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Services.Helpers
{
    public class EquationParser
    {
        #region Regex Constants

        private static Regex unitCombatStatRegex = new Regex(@"{UnitCombatStat\[([A-Za-z ]+)\]}", RegexOptions.Compiled);
        private static Regex unitStatRegex       = new Regex(@"{UnitStat\[([A-Za-z, ]+)\]}", RegexOptions.Compiled);
        private static Regex weaponStatRegex     = new Regex(@"{WeaponStat\[([A-Za-z, ]+)\]}", RegexOptions.Compiled);
        private static Regex battalionStatRegex  = new Regex(@"{BattalionStat\[([A-Za-z, ]+)\]}", RegexOptions.Compiled);

        #endregion Regex Constants

        /// <summary>
        /// Evaluates the dynamic equation contained in <paramref name="equation"/> and returns its unrounded result as a decimal.
        /// </summary>
        public static decimal Evaluate(string equation, Unit unit, EquationParserOptions options)
        {
            if (options.EvalUnitCombatStat) ReplaceUnitCombatStatVariables(ref equation, unit);
            if (options.EvalUnitStat) ReplaceUnitStatVariables(ref equation, unit);
            if (options.EvalUnitLevel) ReplaceUnitLevelVariables(ref equation, unit);
            if (options.EvalWeaponUtilStat) ReplaceWeaponUtilStatVariables(ref equation, unit);
            if (options.EvalWeaponStat) ReplaceWeaponStatVariables(ref equation, unit);
            if (options.EvalBattalionStat) ReplaceBattalionStatVariables(ref equation, unit);

            //Throw an error if anything remains unparsed
            if (equation.Contains("{") || equation.Contains("}"))
                throw new UnrecognizedEquationVariableException(equation);

            try
            {
                Expression expression = new Expression(equation);
                return Convert.ToDecimal(expression.Evaluate());
            }
            catch (Exception ex)
            {
                throw new EquationEvaluationErrorException(equation);
            }
        }

        #region Variable Replacement Functions

        /// <summary>
        /// Replaces all instances of {UnitCombatStat[...] with values from the <paramref name="unit"/>'s Combat stats list.
        /// </summary>
        /// <remarks>
        /// ORDER DEPENDENT. This will only work properly if <paramref name="equation"/> is being evaluated after the specified stat has been evaluated itself.
        /// Make sure the Combat Stats config list is ordered correctly if this is being used!
        /// </remarks>
        private static void ReplaceUnitCombatStatVariables(ref string equation, Unit unit)
        {
            MatchCollection unitCombatStatMatches = unitCombatStatRegex.Matches(equation);
            if (unitCombatStatMatches.Count == 0)
                return;

            foreach (Match match in unitCombatStatMatches)
            {
                string statName = match.Groups[1].Value.Trim();

                ModifiedStatValue unitCombatStat;
                if (!unit.Stats.Combat.TryGetValue(statName, out unitCombatStat))
                    throw new UnmatchedStatException(statName);

                equation = equation.Replace(match.Groups[0].Value, unitCombatStat.FinalValue.ToString());
            }
        }

        /// <summary>
        /// Replaces all instances of {UnitStat[...]} with values from the <paramref name="unit"/>'s General stats list.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        private static void ReplaceUnitStatVariables(ref string equation, Unit unit)
        {
            MatchCollection unitStatMatches = unitStatRegex.Matches(equation);
            if (unitStatMatches.Count == 0)
                return;

            foreach (Match match in unitStatMatches)
            {
                int maximumStatValue = int.MinValue;
                foreach (string statSplit in match.Groups[1].Value.Split(","))
                {
                    ModifiedStatValue unitStat;
                    if (!unit.Stats.General.TryGetValue(statSplit.Trim(), out unitStat))
                        throw new UnmatchedStatException(statSplit);

                    if (maximumStatValue < unitStat.FinalValue)
                        maximumStatValue = unitStat.FinalValue;
                }

                equation = equation.Replace(match.Groups[0].Value, maximumStatValue.ToString());
            }
        }

        /// <summary>
        /// Replaces all instances of {UnitLevel} with the <paramref name="unit"/>'s Level value.
        /// </summary>
        private static void ReplaceUnitLevelVariables(ref string equation, Unit unit)
        {
            equation = equation.Replace("{UnitLevel}", unit.Stats.Level.ToString());
        }

        /// <summary>
        /// Replaces all instances of {WeaponUtilStat} with the unit stat value indicated by the <paramref name="unit"/>'s primary equipped item's utilized stat.
        /// </summary>
        private static void ReplaceWeaponUtilStatVariables(ref string equation, Unit unit)
        {
            if (equation.Contains("{WeaponUtilStat}"))
            {
                string utilizedStat = GetPrimaryEquippedItemUtilizedStatName(unit);

                int value = 0;
                if (!string.IsNullOrEmpty(utilizedStat))
                    value = unit.Stats.General[utilizedStat].FinalValue;

                equation = equation.Replace("{WeaponUtilStat}", value.ToString());
            }
        }

        /// <summary>
        /// Replaces all instances of {WeaponStat[...]} with the matching stat value from the <paramref name="unit"/>'s primary equipped item. If there is no primary equipped weapon, uses 0.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        private static void ReplaceWeaponStatVariables(ref string equation, Unit unit)
        {
            MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
            if (weaponStatMatches.Count == 0)
                return;

            UnitInventoryItem primaryEquipped = unit.Inventory.SingleOrDefault(i => i != null && i.IsPrimaryEquipped);

            foreach (Match match in weaponStatMatches)
            {
                int maximumStatValue = int.MinValue;

                if (primaryEquipped != null)
                {
                    foreach (string statSplit in match.Groups[1].Value.Split(","))
                    {
                        int statValue;
                        if (!primaryEquipped.Item.Stats.TryGetValue(statSplit.Trim(), out statValue))
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

        /// <summary>
        /// Replaces all instances of {BattalionStat[...]} with the matching stat value from the <paramref name="unit"/>'s battalion. If there is no battalion, uses 0.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        private static void ReplaceBattalionStatVariables(ref string equation, Unit unit)
        {
            MatchCollection battalionStatMatches = battalionStatRegex.Matches(equation);
            if (battalionStatMatches.Count == 0)
                return;

            Battalion battalion = unit.Battalion?.BattalionObj;

            foreach (Match match in battalionStatMatches)
            {
                int maximumStatValue = int.MinValue;

                if (battalion != null)
                {
                    foreach (string statSplit in match.Groups[1].Value.Split(","))
                    {
                        int statValue;
                        if (!battalion.Stats.TryGetValue(statSplit.Trim(), out statValue))
                            throw new UnmatchedStatException(statSplit);

                        if (maximumStatValue < statValue)
                            maximumStatValue = statValue;
                    }
                }
                else
                {
                    //If there is no battalion, default value to 0.
                    maximumStatValue = 0;
                }

                equation = equation.Replace(match.Groups[0].Value, maximumStatValue.ToString());
            }
        }

        #endregion Variable Replacement Functions

        private static string GetPrimaryEquippedItemUtilizedStatName(Unit unit)
        {
            UnitInventoryItem primaryEquipped = unit.Inventory.SingleOrDefault(i => i != null && i.IsPrimaryEquipped);

            string statName = string.Empty;
            int maxValue = int.MinValue;

            if (primaryEquipped == null)
                return statName;

            foreach (string utilStatName in primaryEquipped.Item.UtilizedStats)
            {
                ModifiedStatValue weaponUtilStat;
                if (!unit.Stats.General.TryGetValue(utilStatName, out weaponUtilStat))
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

    public struct EquationParserOptions
    {
        public bool EvalUnitCombatStat;
        public bool EvalUnitStat;
        public bool EvalUnitLevel;
        public bool EvalWeaponUtilStat;
        public bool EvalWeaponStat;
        public bool EvalBattalionStat;
    }
}