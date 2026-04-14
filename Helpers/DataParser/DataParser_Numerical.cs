using RedditEmblemAPI.Models.Exceptions.Validation;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Helpers
{
    /// <summary>
    /// Static class containing functions to assist with data parsing.
    /// </summary>
    public static partial class DataParser
    {
        #region Int_Any

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int Int_Any(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Int_Any(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer.
        /// </summary>
        public static int Int_Any(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_Any(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as an integer.
        /// </summary>
        /// <exception cref="AnyIntegerException"></exception>
        public static int Int_Any(string value, string fieldName)
        {
            if (!int.TryParse(value, out int val))
                throw new AnyIntegerException(fieldName, value);
            return val;
        }

        #endregion Int_Any

        #region Int_Positive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is below 0.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int Int_Positive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Int_Positive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0.
        /// </summary>
        public static int Int_Positive(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_Positive(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="value"/> as an integer. Errors if the value is below 0.
        /// </summary>
        /// <exception cref="PositiveIntegerException"></exception>
        public static int Int_Positive(string value, string fieldName)
        {
            if (!int.TryParse(value, out int val) || val < 0)
                throw new PositiveIntegerException(fieldName, value);
            return val;
        }

        #endregion Int_Positive

        #region Int_NonZeroPositive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is below 1.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int Int_NonZeroPositive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Int_NonZeroPositive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1.
        /// </summary>
        public static int Int_NonZeroPositive(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_NonZeroPositive(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> as an integer. Errors if the value is below 1.
        /// </summary>
        /// <exception cref="NonZeroPositiveIntegerException"></exception>
        public static int Int_NonZeroPositive(string value, string fieldName)
        {
            if (!int.TryParse(value, out int val) || val < 1)
                throw new NonZeroPositiveIntegerException(fieldName, value);
            return val;
        }

        #endregion Int_NonZeroPositive

        #region Int_Negative

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is above -1.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int Int_Negative(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Int_Negative(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1.
        /// </summary>
        public static int Int_Negative(IEnumerable<string> data, int index, string fieldName)
        {
            return Int_Negative(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> as an integer. Errors if the value is above -1.
        /// </summary>
        /// <exception cref="NegativeIntegerException"></exception>
        public static int Int_Negative(string value, string fieldName)
        {
            if (!int.TryParse(value, out int val) || val > -1)
                throw new NegativeIntegerException(fieldName, value);
            return val;
        }

        #endregion Int_Negative

        #region OptionalInt_Any

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int OptionalInt_Any(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, int defaultValueIfNull = 0)
        {
            return OptionalInt_Any(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Any(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Int_Any(value, fieldName);
        }

        #endregion OptionalInt_Any

        #region OptionalInt_Positive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int OptionalInt_Positive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, int defaultValueIfNull = 0)
        {
            return OptionalInt_Positive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Positive(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 0)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Int_Positive(value, fieldName);
        }

        #endregion OptionalInt_Positive

        #region OptionalInt_NonZeroPositive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is below 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int OptionalInt_NonZeroPositive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, int defaultValueIfNull = 1)
        {
            return OptionalInt_NonZeroPositive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is below 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_NonZeroPositive(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = 1)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Int_NonZeroPositive(value, fieldName);
        }

        #endregion OptionalInt_NonZeroPositive

        #region OptionalInt_Negative

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is above -1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static int OptionalInt_Negative(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, int defaultValueIfNull = -1)
        {
            return OptionalInt_Negative(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above -1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static int OptionalInt_Negative(IEnumerable<string> data, int index, string fieldName, int defaultValueIfNull = -1)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Int_Negative(value, fieldName);
        }

        #endregion OptionalInt_Negative

        #region Decimal_Any

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal Decimal_Any(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Decimal_Any(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal.
        /// </summary>
        public static decimal Decimal_Any(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Any(data?.ElementAtOrDefault(index), fieldName);
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

        #endregion Decimal_Any

        #region Decimal_Positive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is below 0.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal Decimal_Positive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Decimal_Positive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below 0.
        /// </summary>
        public static decimal Decimal_Positive(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Positive(data?.ElementAtOrDefault(index), fieldName);
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

        #endregion Decimal_Positive

        #region Decimal_NonZeroPositive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is below or equal to 0.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal Decimal_NonZeroPositive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Decimal_NonZeroPositive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below or equal to 0.
        /// </summary>
        public static decimal Decimal_NonZeroPositive(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_NonZeroPositive(data?.ElementAtOrDefault(index), fieldName);
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

        #endregion Decimal_NonZeroPositive

        #region Decimal_OneOrGreater

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is less than 1.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal Decimal_OneOrGreater(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Decimal_OneOrGreater(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is less than 1.
        /// </summary>
        public static decimal Decimal_OneOrGreater(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_OneOrGreater(data?.ElementAtOrDefault(index), fieldName); 
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

        #endregion Decimal_OneOrGreater

        #region Decimal_Negative

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as an integer. Errors if the value is above or equal to 0.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal Decimal_Negative(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return Decimal_Negative(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as an integer. Errors if the value is above or equal to 0.
        /// </summary>
        public static decimal Decimal_Negative(IEnumerable<string> data, int index, string fieldName)
        {
            return Decimal_Negative(data?.ElementAtOrDefault(index), fieldName);
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

        #endregion Decimal_Negative

        #region OptionalDecimal_Any

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal OptionalDecimal_Any(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, decimal defaultValueIfNull = 0)
        {
            return OptionalDecimal_Any(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Any(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Decimal_Any(value, fieldName);
        }

        #endregion OptionalDecimal_Any

        #region OptionalDecimal_Positive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal OptionalDecimal_Positive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, decimal defaultValueIfNull = 0)
        {
            return OptionalDecimal_Positive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Positive(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 0)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Decimal_Positive(value, fieldName);
        }

        #endregion OptionalDecimal_Positive

        #region OptionalDecimal_NonZeroPositive

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is below or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal OptionalDecimal_NonZeroPositive(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, decimal defaultValueIfNull = 1)
        {
            return OptionalDecimal_NonZeroPositive(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is below or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_NonZeroPositive(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 1)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Decimal_NonZeroPositive(value, fieldName);
        }

        #endregion OptionalDecimal_NonZeroPositive

        #region OptionalDecimal_OneOrGreater

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is less than 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal OptionalDecimal_OneOrGreater(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, decimal defaultValueIfNull = 1)
        {
            return OptionalDecimal_OneOrGreater(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is less than 1. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_OneOrGreater(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = 1)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Decimal_OneOrGreater(value, fieldName);
        }

        #endregion OptionalDecimal_OneOrGreater

        #region OptionalDecimal_Negative

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="indices"/> as a decimal. Errors if the value is above or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static decimal OptionalDecimal_Negative(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName, decimal defaultValueIfNull = -1)
        {
            return OptionalDecimal_Negative(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName, defaultValueIfNull);
        }

        /// <summary>
        /// Returns the numerical value in <paramref name="data"/> at <paramref name="index"/> as a decimal. Errors if the value is above or equal to 0. If the value is empty, returns <paramref name="defaultValueIfNull"/> instead.
        /// </summary>
        public static decimal OptionalDecimal_Negative(IEnumerable<string> data, int index, string fieldName, decimal defaultValueIfNull = -1)
        {
            string value = data?.ElementAtOrDefault(index);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValueIfNull;

            return Decimal_Negative(value, fieldName);
        }

        #endregion OptionalDecimal_Negative

    }
}