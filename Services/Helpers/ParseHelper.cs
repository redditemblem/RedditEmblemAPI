using RedditEmblemAPI.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Services.Helpers
{
    public static class ParseHelper
    {
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

        public static int OptionalSafeIntParse(string number, string fieldName, bool isPositive, int defaultValueIfNull)
        {
            if (string.IsNullOrEmpty(number))
                return defaultValueIfNull;

            return SafeIntParse(number, fieldName, isPositive);
        }

        public static IList<string> StringListParse(IList<string> data, IList<int> range, bool keepEmptyValues = false)
        {
            IList<string> output = new List<string>();
            foreach (int field in range)
                if (!string.IsNullOrEmpty(data.ElementAtOrDefault<string>(field)) || keepEmptyValues)
                    output.Add(data.ElementAtOrDefault<string>(field).Trim());

            return output;
        }

        public static IList<string> StringCSVParse(IList<string> data, int field, bool keepEmptyValues = false)
        {
            IList<string> output = new List<string>();
            foreach (string value in data.ElementAtOrDefault<string>(field).Split(','))
                if (!string.IsNullOrEmpty(value) || keepEmptyValues)
                    output.Add(value.Trim());

            return output;
        }
    }
}
