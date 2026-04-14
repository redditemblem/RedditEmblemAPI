using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Output;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers
{
    public static partial class DataParser
    {
        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, int> NamedStatDictionary_Int_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<IEnumerable<string>> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = Int_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
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
        public static IDictionary<string, int> NamedStatDictionary_OptionalInt_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<IEnumerable<string>> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = OptionalInt_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
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
        public static IDictionary<string, int> NamedStatDictionary_Int_NonZeroPositive(IEnumerable<NamedStatConfig> configs, IEnumerable<IEnumerable<string>> data, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, int> stats = new Dictionary<string, int>();

            foreach (NamedStatConfig stat in configs)
            {
                int val = Int_NonZeroPositive(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                stats.Add(stat.SourceName, val);
            }

            return stats;
        }

        /// <summary>
        /// Iterates the <c>NamedStatConfig</c>s in <paramref name="configs"/> and parses them into a dictionary. All values are included.
        /// </summary>
        /// <param name="errorFieldNameFormat">Passed into <c>string.Format</c> to create the field name thrown with any parsing errors. The <c>NamedStatConfig</c>'s source name is always {0}.</param>
        ///<param name="errorFieldNameArgs">Any additional values that will be formatted with <paramref name="errorFieldNameFormat"/>.</param>
        public static IDictionary<string, decimal> NamedStatDictionary_Decimal_Any(IEnumerable<NamedStatConfig> configs, IEnumerable<IEnumerable<string>> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, decimal> stats = new Dictionary<string, decimal>();

            foreach (NamedStatConfig stat in configs)
            {
                decimal val = Decimal_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
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
        public static IDictionary<string, INamedStatValue> NamedStatValueDictionary_OptionalDecimal_Any(IEnumerable<NamedStatConfig_Displayed> configs, IEnumerable<IEnumerable<string>> data, bool includeZeroValues = false, string errorFieldNameFormat = "{0}", params string[] errorFieldNameArgs)
        {
            IDictionary<string, INamedStatValue> stats = new Dictionary<string, INamedStatValue>();

            foreach (NamedStatConfig_Displayed stat in configs)
            {
                decimal val = OptionalDecimal_Any(data, stat.Value, string.Format(errorFieldNameFormat, errorFieldNameArgs.Prepend(stat.SourceName).ToArray()));
                if (val == 0 && !includeZeroValues) continue;

                stats.Add(stat.SourceName, new NamedStatValue(val, stat.InvertModifiedDisplayColors));
            }

            return stats;
        }
    }
}
