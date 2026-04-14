using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers
{
    public static partial class DataParser
    {
        /// <summary>
        /// Returns a list containing the values of <paramref name="data"/> at the locations contained in <paramref name="indices"/>.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_Strings(IEnumerable<IEnumerable<string>> data, IEnumerable<(int, int)> indices, bool keepEmptyValues = false)
        {
            List<string> output = new List<string>();
            foreach ((int, int) index in indices)
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
        /// Splits the CSVs contained in <paramref name="data"/> at the locations in <paramref name="indices"/> and flattens them into one list.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_StringCSV(IEnumerable<IEnumerable<string>> data, IEnumerable<(int, int)> indices, bool keepEmptyValues = false)
        {
            return indices.SelectMany(index => List_StringCSV(data?.ElementAtOrDefault(index.Item1), index.Item2, keepEmptyValues)).ToList();
        }

        /// <summary>
        /// Splits the CSV contained in <paramref name="data"/> at index <paramref name="index"/> and flattens it into a list.
        /// </summary>
        /// <param name="keepEmptyValues">If true, then null or empty string values from <paramref name="data"/> will be retained in the returned list.</param>
        public static List<string> List_StringCSV(IEnumerable<string> data, int index, bool keepEmptyValues = false)
        {
            return List_StringCSV(data?.ElementAtOrDefault(index), keepEmptyValues);
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
        /// Converts the CSV in <paramref name="data"/> at <paramref name="indices"/> to a list of integers.
        /// </summary>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        public static List<int> List_IntCSV(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, bool isPositive)
        {
            return List_IntCSV(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, isPositive);
        }

        /// <summary>
        /// Converts the CSV in <paramref name="data"/> at <paramref name="index"/> to a list of integers.
        /// </summary>
        /// <param name="fieldName">The name of the numerical value list as it should display in any thrown exception messages.</param>
        /// <param name="isPositive">If true, an exception will be thrown if any integer in the CSV is less than 0.</param>
        public static List<int> List_IntCSV(IEnumerable<string> data, int index, string fieldName, bool isPositive)
        {
            return List_IntCSV(data?.ElementAtOrDefault(index), fieldName, isPositive);
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
    }
}
