﻿using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Services.Helpers
{
    /// <summary>
    /// Static class containing functions to assist with data parsing.
    /// </summary>
    public static class DataParser
    {
        #region Constants

        private static readonly Regex hexRegex = new Regex("^#?([0-9A-Fa-f]{6}$)");

        #endregion Constants

        #region Numerical Parsing

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer.
        /// </summary>
        public static int Int_Any(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_Any(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as an integer.
        /// </summary>
        /// <exception cref="AnyIntegerException"></exception>
        public static int Int_Any(string value, string fieldName)
        {
            int val;
            if (!int.TryParse(value, out val))
                throw new AnyIntegerException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0.
        /// </summary>
        public static int Int_Positive(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_Positive(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as an integer. Errors if the value is below 0.
        /// </summary>
        /// <exception cref="PositiveIntegerException"></exception>
        public static int Int_Positive(string value, string fieldName)
        {
            int val;
            if (!int.TryParse(value, out val) || val < 0)
                throw new PositiveIntegerException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1.
        /// </summary>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public static int Int_NonZeroPositive(IEnumerable<string> data, int index, string fieldName)
        {
            string number = data.ElementAtOrDefault<string>(index);

            int val;
            if (!int.TryParse(number, out val) || val < 1)
                throw new NonZeroPositiveIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1.
        /// </summary>
        /// <exception cref="NegativeIntegerException"></exception>
        public static int Int_Negative(IEnumerable<string> data, int index, string fieldName)
        {
            string number = data.ElementAtOrDefault<string>(index);

            int val;
            if (!int.TryParse(number, out val) || val > -1)
                throw new NegativeIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Any(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Any(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Positive(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Positive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_NonZeroPositive(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 1)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_NonZeroPositive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Negative(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = -1)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Negative(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal.
        /// </summary>
        public static decimal Decimal_Any(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Any(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as a decimal.
        /// </summary>
        /// <exception cref="AnyDecimalException"></exception>
        public static decimal Decimal_Any(string value, string fieldName)
        {
            decimal val;
            if (!decimal.TryParse(value, out val))
                throw new AnyDecimalException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below 0.
        /// </summary>
        public static decimal Decimal_Positive(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Positive(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as a decimal. Errors if the value is below 0.
        /// </summary>
        /// <exception cref="PositiveDecimalException"></exception>
        public static decimal Decimal_Positive(string value, string fieldName)
        {
            decimal val;
            if (!decimal.TryParse(value, out val) || val < 0)
                throw new PositiveDecimalException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below or equal to 0.
        /// </summary>
        public static decimal Decimal_NonZeroPositive(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_NonZeroPositive(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as a decimal. Errors if the value is below or equal to 0.
        /// </summary>
        /// <exception cref="NonZeroPositiveDecimalException"></exception>
        public static decimal Decimal_NonZeroPositive(string value, string fieldName)
        {
            decimal val;
            if (!decimal.TryParse(value, out val) || val <= 0)
                throw new NonZeroPositiveDecimalException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is less than 1.
        /// </summary>
        public static decimal Decimal_OneOrGreater(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_OneOrGreater(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName); 
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as a decimal. Errors if that value is less than 1.
        /// </summary>
        /// <exception cref="OneOrGreaterDecimalException"></exception>
        public static decimal Decimal_OneOrGreater(string value, string fieldName)
        {
            decimal val;
            if (!decimal.TryParse(value, out val) || val < 1)
                throw new OneOrGreaterDecimalException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above or equal to 0.
        /// </summary>
        public static decimal Decimal_Negative(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Negative(data.ElementAtOrDefault<string>(index) ?? string.Empty, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as a decimal. Errors if the value is above or equal to 0.
        /// </summary>
        /// <exception cref="NegativeDecimalException"></exception>
        public static decimal Decimal_Negative(string value, string fieldName)
        {
            decimal val;
            if (!decimal.TryParse(value, out val) || val >= 0)
                throw new NegativeDecimalException(fieldName, value);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Any(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Any(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Positive(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Positive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_NonZeroPositive(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 1)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_NonZeroPositive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is less than 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_OneOrGreater(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 1)
        {
            if(string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_OneOrGreater(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is above or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Negative(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = -1)
        {
            if (string.IsNullOrWhiteSpace(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Negative(data, index, fieldName);
        }

        #endregion

        #region String Parsing

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        public static string String(IEnumerable<string> data, int index, string fieldName)
        {
            return String(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public static string String(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new RequiredValueNotProvidedException(fieldName);

            return value.Trim();
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim();
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a formatted URL.
        /// </summary>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        public static string String_URL(IEnumerable<string> data, int index, string fieldName)
        {
            return String_URL(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a formatted URL.
        /// </summary>
        /// <exception cref="URLException"></exception>
        public static string String_URL(string value, string fieldName)
        {
            value = String(value, fieldName);

            //Validate that this string is a URL
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                throw new URLException(fieldName, value);

            return value;
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a formatted URL.
        /// </summary>
        public static string OptionalString_URL(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString_URL(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a formatted URL.
        /// </summary>
        /// <exception cref="URLException"></exception>
        public static string OptionalString_URL(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            //Validate that this string is a URL
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                throw new URLException(fieldName, value);

            return value;
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a hex code.
        /// </summary>
        public static string String_HexCode(IEnumerable<string> data, int index, string fieldName)
        {
            return String_HexCode(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a hex code.
        /// </summary>
        /// <exception cref="HexException"></exception>
        public static string String_HexCode(string value, string fieldName)
        {
            value = String(value, fieldName);

            //Validate that this string is a hex code
            Match match = hexRegex.Match(value);
            if (!match.Success)
                throw new HexException(fieldName, value);

            //Return hex formatted with a # symbol
            return $"#{match.Groups[1]}";
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a hex color code.
        /// </summary>
        public static string OptionalString_HexCode(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString_HexCode(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a hex color code.
        /// </summary>
        /// <exception cref="HexException"></exception>
        public static string OptionalString_HexCode(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            //Validate that this string is a hex code
            Match match = hexRegex.Match(value);
            if (!match.Success)
                throw new HexException(fieldName, value);

            //Return hex formatted with a # symbol
            return $"#{match.Groups[1]}";
        }

        #endregion

        #region List Parsing

        /// <summary>
        /// Returns a list containing the values of <paramref name="data"/> at the locations contained in <paramref name="indexes"/>.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_Strings(IEnumerable<string> data, List<int> indexes, bool keepEmptyValues = false)
        {
            List<string> output = new List<string>();
            foreach (int index in indexes)
            {
                string value = OptionalString(data, index, string.Empty);
                if (!string.IsNullOrWhiteSpace(value))
                    output.Add(value);
                else if (keepEmptyValues)
                    output.Add(string.Empty);
            }

            return output;
        }

        /// <summary>
        /// Splits the CSVs contained in <paramref name="data"/> at the indexes in <paramref name="indexes"/> and formats them into one list.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_StringCSV(IEnumerable<string> data, List<int> indexes, bool keepEmptyValues = false)
        {
            return indexes.SelectMany(index => List_StringCSV(data, index, keepEmptyValues)).ToList();
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="data"/> at index <paramref name="index"/> and formats it into a list.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_StringCSV(IEnumerable<string> data, int index, bool keepEmptyValues = false)
        {
            return List_StringCSV((data.ElementAtOrDefault<string>(index) ?? string.Empty), keepEmptyValues);
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="csv"/> and formats it into a list.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values will be retained in the returned list.</param>
        public static List<string> List_StringCSV(string csv, bool keepEmptyValues = false)
        {
            List<string> output = new List<string>();

            if (string.IsNullOrWhiteSpace(csv))
                return output;

            foreach (string csvItem in csv.Split(','))
            {
                string value = OptionalString(csvItem, string.Empty);
                if (!string.IsNullOrWhiteSpace(value))
                    output.Add(value);
                else if (keepEmptyValues)
                    output.Add(string.Empty);
            }

            return output;
        }

        /// <summary>
        /// Converts the CSV in <paramref name="data"/> at <paramref name="index"/> to a list of integers.
        /// </summary>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        public static List<int> List_IntCSV(IEnumerable<string> data, int index, string fieldName, bool isPositive)
        {
            return List_IntCSV(data.ElementAtOrDefault<string>(index), fieldName, isPositive);
        }

        /// <summary>
        /// Converts the CSV in <paramref name="csv"/> to a list of integers.
        /// </summary>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        public static List<int> List_IntCSV(string csv, string fieldName, bool isPositive)
        {
            List<int> output = new List<int>();

            if (string.IsNullOrWhiteSpace(csv))
                return output;

            foreach (string value in csv.Split(','))
            {
                int val = 0;
                if (isPositive) val = Int_Positive(value, fieldName);
                else val = Int_Any(value, fieldName);

                output.Add(val);
            }

            return output;
        }

        #endregion

        #region Boolean Parsing

        /// <summary>
        /// Returns true if the value in <paramref name="data"/> at <paramref name="index"/> is equal to "yes".
        /// </summary>
        public static bool OptionalBoolean_YesNo(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalBoolean_YesNo(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns true if <paramref name="value"/> is equal to "yes".
        /// </summary>
        public static bool OptionalBoolean_YesNo(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            return value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        #endregion Boolean Parsing

        #region Stat Dictionary Builders

        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, int> NamedStatDictionary_Int_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<string> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.Int_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                if (val == 0 && !includeZeroValues) continue;

                stats.Add(stat.SourceName, val);
            }

            return stats;
        }

        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, int> NamedStatDictionary_OptionalInt_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<string> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.OptionalInt_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                if (val == 0 && !includeZeroValues) continue;

                stats.Add(stat.SourceName, val);
            }

            return stats;
        }

        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, int> NamedStatDictionary_Int_NonZeroPositive(IEnumerable<NamedStatConfig> configs, IEnumerable<string> data, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.Int_NonZeroPositive(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                stats.Add(stat.SourceName, val);
            }

            return stats;
        }

        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, decimal> NamedStatDictionary_Decimal_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<string> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, decimal> stats = new Dictionary<string, decimal>();

            foreach (NamedStatConfig stat in configs)
            {
                decimal val = DataParser.Decimal_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                if (val == 0 && !includeZeroValues) continue;

                stats.Add(stat.SourceName, val);
            }

            return stats;
        }

        /// <summary>
        /// Iterates the <c>NamedStatConfig_Displayed</c> items in <paramref name="configs"/> and parses them into a dictionary of <c>NamedStatValues</c>.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, NamedStatValue> NamedStatValueDictionary_OptionalDecimal_Any(IEnumerable<NamedStatConfig_Displayed> configs, IEnumerable<string> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, NamedStatValue> stats = new Dictionary<string, NamedStatValue>();

            foreach (NamedStatConfig_Displayed stat in configs)
            {
                decimal val = DataParser.OptionalDecimal_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                if (val == 0 && !includeZeroValues) continue;

                stats.Add(stat.SourceName, new NamedStatValue(val, stat.InvertModifiedDisplayColors));
            }

            return stats;
        }

        /// <summary>
        /// Returns the string CSV at <paramref name="statsIndex"/> and the int CSV at <paramref name="valuesIndex"/> in <paramref name="data"/> as a dictionary.
        /// </summary>
        public static IDictionary<string, int> StatValueCSVs_Int_Any(IEnumerable<string> data, int statsIndex, string statsFieldName, int valuesIndex, string valuesFieldName, bool includeZeroValues = false)
        {
            List<string> stats = DataParser.List_StringCSV(data, statsIndex);
            List<int> values = DataParser.List_IntCSV(data, valuesIndex, valuesFieldName, false);

            return StatValueCSVs_Int_Any(stats, statsFieldName, values, valuesFieldName, includeZeroValues);
        }

        /// <summary>
        /// Returns a dictionary where <paramref name="stats"/> are keys and <paramref name="values"/> are values.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException">Thrown if either <paramref name="stats"/> or <paramref name="values"/> is empty.</exception>
        /// <exception cref="ParameterLengthsMismatchedException">Thrown if the lengths of <paramref name="stats"/> and <paramref name="values"/> are not equal.</exception>
        public static IDictionary<string, int> StatValueCSVs_Int_Any(List<string> stats, string statsFieldName, List<int> values, string valuesFieldName, bool includeZeroValues = false)
        {
            if (!stats.Any()) throw new RequiredValueNotProvidedException(statsFieldName);
            if (!values.Any()) throw new RequiredValueNotProvidedException(valuesFieldName);
            if (stats.Count != values.Count) throw new ParameterLengthsMismatchedException(statsFieldName, valuesFieldName);

            IDictionary<string, int> modifiers = new Dictionary<string, int>();
            for(int i = 0; i < stats.Count; i++)
            {
                if (values[i] == 0 && !includeZeroValues)
                    continue;

                modifiers.Add(stats[i], values[i]);
            }

            return modifiers;
        }

        #endregion Stat Dictionary Builders
    }
}