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
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault<string>(index)) || keepEmptyValues)
                    output.Add(data.ElementAtOrDefault<string>(index).Trim());

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
            IList<string> output = new List<string>();
            
            foreach (string value in (data.ElementAtOrDefault<string>(index) ?? string.Empty).Split(','))
                if (!string.IsNullOrEmpty(value) || keepEmptyValues)
                    output.Add(value.Trim());

            return output;
        }

        #endregion
    }
}
