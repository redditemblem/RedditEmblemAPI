using RedditEmblemAPI.Models.Exceptions.Validation;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers
{
    public static partial class DataParser
    {
        /// <summary>
        /// Returns the string CSV at <paramref name="statsIndex"/> and the int CSV at <paramref name="valuesIndex"/> in <paramref name="data"/> as a dictionary.
        /// </summary>
        public static IDictionary<string, int> StatValueCSVs_Int_Any(IEnumerable<string> data, int statsIndex, string statsFieldName, int valuesIndex, string valuesFieldName, bool includeZeroValues = false)
        {
            List<string> stats = List_StringCSV(data, statsIndex);
            List<int> values = List_IntCSV(data, valuesIndex, valuesFieldName, false);

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
            for (int i = 0; i < stats.Count; i++)
            {
                if (values[i] == 0 && !includeZeroValues)
                    continue;

                modifiers.Add(stats[i], values[i]);
            }

            return modifiers;
        }
    }
}
