using RedditEmblemAPI.Models.Exceptions.Validation;
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
        public static int Int_Any(List<string> data, int index, string fieldName)
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
        public static int Int_Positive(List<string> data, int index, string fieldName)
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
        public static int Int_NonZeroPositive(List<string> data, int index, string fieldName)
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
        public static int Int_Negative(List<string> data, int index, string fieldName)
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
        public static int OptionalInt_Any(List<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Any(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Positive(List<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Positive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_NonZeroPositive(List<string> data, int index, string fieldName, int defaultValueIfNull = 1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_NonZeroPositive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Negative(List<string> data, int index, string fieldName, int defaultValueIfNull = -1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Negative(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal.
        /// </summary>
        public static decimal Decimal_Any(List<string> data, int index, string fieldName)
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
        public static decimal Decimal_Positive(List<string> data, int index, string fieldName)
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
        public static decimal Decimal_NonZeroPositive(List<string> data, int index, string fieldName)
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
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above or equal to 0.
        /// </summary>
        public static decimal Decimal_Negative(List<string> data, int index, string fieldName)
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
        public static decimal OptionalDecimal_Any(List<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Any(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Positive(List<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Positive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_NonZeroPositive(List<string> data, int index, string fieldName, decimal defaultValueIfNull = 1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_NonZeroPositive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is above or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Negative(List<string> data, int index, string fieldName, decimal defaultValueIfNull = -1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Decimal_Negative(data, index, fieldName);
        }

        #endregion

        #region String Parsing

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        public static string String(List<string> data, int index, string fieldName)
        {
            return String(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public static string String(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
                throw new RequiredValueNotProvidedException(fieldName);

            return value.Trim();
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(List<string> data, int index, string fieldName)
        {
            return OptionalString(data.ElementAtOrDefault<string>(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Trim();
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a formatted URL.
        /// </summary>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        public static string String_URL(List<string> data, int index, string fieldName)
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
        public static string OptionalString_URL(List<string> data, int index, string fieldName)
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

            if (string.IsNullOrEmpty(value))
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
        public static string String_HexCode(List<string> data, int index, string fieldName)
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
        public static string OptionalString_HexCode(List<string> data, int index, string fieldName)
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

            if (string.IsNullOrEmpty(value))
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
        public static List<string> List_Strings(List<string> data, List<int> indexes, bool keepEmptyValues = false)
        {
            List<string> output = new List<string>();
            foreach (int index in indexes)
            {
                string value = OptionalString(data, index, string.Empty);
                if (!string.IsNullOrEmpty(value))
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
        public static List<string> List_StringCSV(List<string> data, List<int> indexes, bool keepEmptyValues = false)
        {
            return indexes.SelectMany(index => List_StringCSV(data, index, keepEmptyValues)).ToList();
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="data"/> at index <paramref name="index"/> and formats it into a list.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_StringCSV(List<string> data, int index, bool keepEmptyValues = false)
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

            if (string.IsNullOrEmpty(csv))
                return output;

            foreach (string csvItem in csv.Split(','))
            {
                string value = OptionalString(csvItem, string.Empty);
                if (!string.IsNullOrEmpty(value))
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
        public static List<int> List_IntCSV(List<string> data, int index, string fieldName, bool isPositive)
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

            if (string.IsNullOrEmpty(csv))
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
        public static bool OptionalBoolean_YesNo(List<string> data, int index, string fieldName)
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
    }
}