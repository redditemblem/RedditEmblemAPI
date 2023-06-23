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
        #region Constants

        private static Regex unitCombatStatRegex = new Regex(@"{UnitCombatStat\[([A-Za-z ]+)\]}");
        private static Regex unitStatRegex = new Regex(@"{UnitStat\[([A-Za-z, ]+)\]}");
        private static Regex weaponStatRegex = new Regex(@"{WeaponStat\[([A-Za-z, ]+)\]}");
        private static Regex battalionStatRegex = new Regex(@"{BattalionStat\[([A-Za-z, ]+)\]}");

        private const string VAR_UNIT_LEVEL = "{UnitLevel}";
        private const string VAR_WEAPON_UTIL_STAT_GREATEST = "{WeaponUtilStat_Greatest}";
        private const string VAR_WEAPON_UTIL_STAT_SUM = "{WeaponUtilStat_Sum}";

        #endregion Constants

        /// <summary>
        /// Evaluates the dynamic equation contained in <paramref name="equation"/> and returns its unrounded result as a decimal.
        /// </summary>
        public static decimal Evaluate(string equation, Unit unit, EquationParserOptions options)
        {
            if (options.EvalUnitCombatStat) ReplaceUnitCombatStatVariables(ref equation, unit);
            if (options.EvalUnitStat) ReplaceUnitStatVariables(ref equation, unit);
            if (options.EvalUnitLevel) ReplaceUnitLevelVariables(ref equation, unit);
            if (options.EvalWeaponUtilStat_Greatest) ReplaceWeaponUtilStatGreatestVariables(ref equation, unit);
            if (options.EvalWeaponUtilStat_Sum) ReplaceWeaponUtilStatSumVariables(ref equation, unit);
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
            catch (Exception)
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
            if (!unitCombatStatMatches.Any()) return;

            foreach (Match match in unitCombatStatMatches)
            {
                string statName = match.Groups[1].Value.Trim();

                ModifiedStatValue unitCombatStat = unit.Stats.MatchCombatStatName(statName);
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
            if (!unitStatMatches.Any()) return;

            foreach (Match match in unitStatMatches)
            {
                int maximumStatValue = int.MinValue;
                foreach (string statSplit in match.Groups[1].Value.Split(","))
                {
                    ModifiedStatValue unitStat = unit.Stats.MatchGeneralStatName(statSplit.Trim());
                    if (maximumStatValue < unitStat.FinalValue)
                        maximumStatValue = unitStat.FinalValue;
                }

                equation = equation.Replace(match.Groups[0].Value, maximumStatValue.ToString());
            }
        }

        /// <summary>
        /// Replaces all instances of <c>VAR_UNIT_LEVEL</c> with the <paramref name="unit"/>'s Level value.
        /// </summary>
        private static void ReplaceUnitLevelVariables(ref string equation, Unit unit)
        {
            equation = equation.Replace(VAR_UNIT_LEVEL, unit.Stats.Level.ToString());
        }

        /// <summary>
        /// Replaces all instances of <c>VAR_WEAPON_UTIL_STAT_GREATEST</c> with the unit stat value indicated by the <paramref name="unit"/>'s primary equipped item's utilized stat.
        /// </summary>
        private static void ReplaceWeaponUtilStatGreatestVariables(ref string equation, Unit unit)
        {
            if (!equation.Contains(VAR_WEAPON_UTIL_STAT_GREATEST))
                return;

            int maxValue = int.MinValue;
            UnitInventoryItem primaryEquipped = GetPrimaryEquippedItem(unit);

            //Take the greatest stat value of all the utilized stats
            if(primaryEquipped != null)
            {
                foreach (string utilizedStat in primaryEquipped.Item.UtilizedStats)
                {
                    ModifiedStatValue unitStat = unit.Stats.MatchGeneralStatName(utilizedStat);
                    if (unitStat.FinalValue > maxValue)
                        maxValue = unitStat.FinalValue;
                }
            }
            
            //Default to 0 if no utilized stats were found
            if (maxValue == int.MinValue)
                maxValue = 0;

            equation = equation.Replace(VAR_WEAPON_UTIL_STAT_GREATEST, maxValue.ToString());

        }

        /// <summary>
        /// Replaces all instances of <c>VAR_WEAPON_UTIL_STAT_SUM</c> with the sum of the unit stat value(s) indicated by the <paramref name="unit"/>'s primary equipped item's utilized stat(s).
        /// </summary>
        private static void ReplaceWeaponUtilStatSumVariables(ref string equation, Unit unit)
        {
            if (!equation.Contains(VAR_WEAPON_UTIL_STAT_SUM))
                return;

            int value = 0;
            UnitInventoryItem primaryEquipped = GetPrimaryEquippedItem(unit);

            if(primaryEquipped != null)
            {
                foreach (string utilizedStat in primaryEquipped.Item.UtilizedStats)
                    value += unit.Stats.MatchGeneralStatName(utilizedStat).FinalValue;
            }

            equation = equation.Replace(VAR_WEAPON_UTIL_STAT_SUM, value.ToString());
        }

        /// <summary>
        /// Replaces all instances of {WeaponStat[...]} with the matching stat value from the <paramref name="unit"/>'s primary equipped item. If there is no primary equipped weapon, uses 0.
        /// </summary>
        /// <exception cref="UnmatchedStatException"></exception>
        private static void ReplaceWeaponStatVariables(ref string equation, Unit unit)
        {
            MatchCollection weaponStatMatches = weaponStatRegex.Matches(equation);
            if (!weaponStatMatches.Any()) return;

            UnitInventoryItem primaryEquipped = unit.Inventory.GetPrimaryEquippedItem();
            if(primaryEquipped == null && unit.Emblem != null)
            {
                //If the primary equipped item isn't in the unit's inventory, check emblem weapons
                primaryEquipped = unit.Emblem.EngageWeapons.SingleOrDefault(i => i.IsPrimaryEquipped);
            }

            foreach (Match match in weaponStatMatches)
            {
                decimal maximumStatValue = decimal.MinValue;

                if (primaryEquipped != null)
                {
                    foreach (string statSplit in match.Groups[1].Value.Split(","))
                    {
                        UnitInventoryItemStat stat = primaryEquipped.MatchStatName(statSplit.Trim());
                        if (maximumStatValue < stat.FinalValue)
                            maximumStatValue = stat.FinalValue;
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
            if (!battalionStatMatches.Any()) return;

            Battalion battalion = unit.Battalion?.BattalionObj;

            foreach (Match match in battalionStatMatches)
            {
                int maximumStatValue = int.MinValue;

                if (battalion != null)
                {
                    foreach (string statSplit in match.Groups[1].Value.Split(","))
                    {
                        int statValue = battalion.MatchStatName(statSplit.Trim());
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

        private static UnitInventoryItem GetPrimaryEquippedItem(Unit unit)
        {
            UnitInventoryItem primaryEquipped = unit.Inventory.GetPrimaryEquippedItem();
            if (primaryEquipped == null && unit.Emblem != null)
            {
                //If the primary equipped item isn't in the unit's inventory, check emblem weapons
                primaryEquipped = unit.Emblem.EngageWeapons.SingleOrDefault(i => i.IsPrimaryEquipped);
            }

            return primaryEquipped;
        }
    }

    public struct EquationParserOptions
    {
        public bool EvalUnitCombatStat;
        public bool EvalUnitStat;
        public bool EvalUnitLevel;
        public bool EvalWeaponUtilStat_Greatest;
        public bool EvalWeaponUtilStat_Sum;
        public bool EvalWeaponStat;
        public bool EvalBattalionStat;
    }
}