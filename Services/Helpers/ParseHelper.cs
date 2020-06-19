using RedditEmblemAPI.Models.Exceptions.Validation;
using System;
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
        /// Converts the value of <paramref name="number"/> to a integer.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fieldName">The name of the numerical value as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if <paramref name="number"/> is less than 0.</param>
        /// <returns></returns>
        /// <exception cref="AnyIntegerException"></exception>
        /// <exception cref="PositiveIntegerException"></exception>
        public static int SafeIntParse(string number, string fieldName, bool isPositive)
        {
            int val;
            if (!int.TryParse(number, out val))
            {
                if (isPositive) throw new PositiveIntegerException(fieldName, number);
                else throw new AnyIntegerException(fieldName, number);
            }
            else if (isPositive && val < 0)
                throw new PositiveIntegerException(fieldName, number);
            return val;
        }

        /// <summary>
        /// Converts the value of <paramref name="number"/> to an integer. If <paramref name="number"/> is null/empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fieldName">The name of the numerical value as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if <paramref name="number"/> is less than 0.</param>
        /// <param name="defaultValueIfNull">The value to return if <paramref name="number"/> is null or empty.</param>
        /// <returns></returns>
        public static int OptionalSafeIntParse(string number, string fieldName, bool isPositive, int defaultValueIfNull)
        {
            if (string.IsNullOrEmpty(number))
                return defaultValueIfNull;

            return SafeIntParse(number, fieldName, isPositive);
        }

        /// <summary>
        /// Converts the CSV in <paramref name="data"/> at <paramref name="index"/> to a list of ints.
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
        /// Converts the CSV in <paramref name="csv"/> to a list of ints.
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
                if (string.IsNullOrEmpty(value))
                    continue;

                int val;
                if(!int.TryParse(value, out val) || (val < 0 && isPositive))
                {
                    if (isPositive) throw new PositiveIntegerException(fieldName, value);
                    else throw new AnyIntegerException(fieldName, value);
                }
                output.Add(val);
            }

            return output;
        }

        #endregion

        #region String Parsing

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
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)))
                    output.Add(data.ElementAtOrDefault<string>(index).Trim());
                else if(keepEmptyValues)
                    output.Add(string.Empty);

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
                if (!string.IsNullOrEmpty(value))
                    output.Add(value.Trim());
                else if (keepEmptyValues)
                    output.Add(string.Empty);

            return output;
        }

        #endregion
    }
}
