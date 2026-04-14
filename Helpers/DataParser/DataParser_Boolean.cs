using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers
{
    public static partial class DataParser
    {
        /// <summary>
        /// Returns true if the value in <paramref name="data"/> at <paramref name="indices"/> is equal to "yes".
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static bool OptionalBoolean_YesNo(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return OptionalBoolean_YesNo(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns true if the value in <paramref name="data"/> at <paramref name="index"/> is equal to "yes".
        /// </summary>
        public static bool OptionalBoolean_YesNo(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalBoolean_YesNo(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns true if <paramref name="value"/> is equal to "yes".
        /// </summary>
        public static bool OptionalBoolean_YesNo(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            return value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
