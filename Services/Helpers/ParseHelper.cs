using RedditEmblemAPI.Models.Exceptions.Validation;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    /// <summary>
    /// Static class containing functions to assist with data parsing.
    /// </summary>
    public static class ParseHelper
    {
        #region Numerical Parsing

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer.
        /// </summary>
        /// <exception cref="AnyIntegerException"></exception>
        public static int Int_Any(IList<string> data, int index, string fieldName)
        {
            string number = data.ElementAtOrDefault<string>(index);

            int val;
            if (!int.TryParse(number, out val))
                throw new AnyIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0.
        /// </summary>
        /// <exception cref="PositiveIntegerException"></exception>
        public static int Int_Positive(IList<string> data, int index, string fieldName)
        {
            string number = data.ElementAtOrDefault<string>(index);

            int val;
            if (!int.TryParse(number, out val) || val < 0)
                throw new PositiveIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1.
        /// </summary>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public static int Int_NonZeroPositive(IList<string> data, int index, string fieldName)
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
        public static int Int_Negative(IList<string> data, int index, string fieldName)
        {
            string number = data.ElementAtOrDefault<string>(index);

            int val;
            if (!int.TryParse(number, out val) || val > -1)
                throw new NegativeIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer.
        /// </summary>
        public static int OptionalInt_Any(IList<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Any(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0.
        /// </summary>
        public static int OptionalInt_Positive(IList<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Positive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1.
        /// </summary>
        public static int OptionalInt_NonZeroPositive(IList<string> data, int index, string fieldName, int defaultValueIfNull = 1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_NonZeroPositive(data, index, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1.
        /// </summary>
        public static int OptionalInt_Negative(IList<string> data, int index, string fieldName, int defaultValueIfNull = -1)
        {
            if (string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                return defaultValueIfNull;
            return Int_Negative(data, index, fieldName);
        }

        #endregion

        #region String Parsing

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        /// <param name="isRequired">When true, an exception will be thrown if the value is out of range, null, or an empty string.</param>
        /// <returns></returns>
        public static string SafeStringParse(IList<string> data, int index, string fieldName, bool isRequired)
        {
            return SafeStringParse(data.ElementAtOrDefault<string>(index), fieldName, isRequired);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        /// <param name="isRequired">When true, an exception will be thrown if the value is null or an empty string.</param>
        /// <returns></returns>
        public static string SafeStringParse(string value, string fieldName, bool isRequired)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (isRequired) throw new RequiredValueNotProvidedException(fieldName);
                else return string.Empty;
            }

            return value.Trim();
        }

        #endregion

        #region List Parsing

        /// <summary>
        /// Returns a list containing the values of <paramref name="data"/> at the locations contained in <paramref name="indexes"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="indexes"></param>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        /// <returns></returns>
        public static IList<string> StringListParse(IList<string> data, IList<int> indexes, bool keepEmptyValues = false)
        {
            IList<string> output = new List<string>();
            foreach (int index in indexes)
            {
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                    output.Add(data.ElementAtOrDefault<string>(index).Trim());
                else if (keepEmptyValues)
                    output.Add(string.Empty);
            }

            return output;
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="data"/> at index <paramref name="index"/> and formats it into a list.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        /// <returns></returns>
        public static IList<string> StringCSVParse(IList<string> data, int index, bool keepEmptyValues = false)
        {
            return StringCSVParse((data.ElementAtOrDefault<string>(index) ?? string.Empty), keepEmptyValues);
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="csv"/> and formats it into a list.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="keepEmptyValues">If true, then null or empty string values will be retained in the returned list.</param>
        /// <returns></returns>
        public static IList<string> StringCSVParse(string csv, bool keepEmptyValues = false)
        {
            IList<string> output = new List<string>();

            if (string.IsNullOrEmpty(csv))
                return output;

            foreach (string value in csv.Split(','))
            {
                if (!string.IsNullOrEmpty(value))
                    output.Add(value.Trim());
                else if (keepEmptyValues)
                    output.Add(string.Empty);
            }

            return output;
        }

        /// <summary>
        /// Converts the CSV in <paramref name="data"/> at <paramref name="index"/> to a list of integers.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        /// <returns></returns>
        public static IList<int> IntCSVParse(IList<string> data, int index, string fieldName, bool isPositive)
        {
            return IntCSVParse(data.ElementAtOrDefault<string>(index), fieldName, isPositive);
        }

        /// <summary>
        /// Converts the CSV in <paramref name="csv"/> to a list of integers.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        /// <exception cref="AnyIntegerException"></exception>
        /// <exception cref="PositiveIntegerException"></exception>
        public static IList<int> IntCSVParse(string csv, string fieldName, bool isPositive)
        {
            IList<int> output = new List<int>();

            if (string.IsNullOrEmpty(csv))
                return output;

            foreach (string value in csv.Split(','))
            {
                int val;
                if (!int.TryParse(value, out val))
                {
                    if (isPositive) throw new PositiveIntegerException(fieldName, value);
                    else throw new AnyIntegerException(fieldName, value);
                }
                else if (isPositive && val < 0)
                    throw new PositiveIntegerException(fieldName, value);

                output.Add(val);
            }

            return output;
        }

        #endregion
    }
}
