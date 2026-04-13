using RedditEmblemAPI.Models.Exceptions.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Helpers
{
    public static partial class DataParser
    {
        #region Constants

        private static readonly Regex hexRegex = new Regex("^#?([0-9A-Fa-f]{6}$)");

        #endregion Constants

        #region String

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> with Trim() applied.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static string String(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return String(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> with Trim() applied.
        /// </summary>
        public static string String(IEnumerable<string> data, int index, string fieldName)
        {
            return String(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        /// <exception cref="RequiredValueNotProvidedException"></exception>
        public static string String(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new RequiredValueNotProvidedException(fieldName);

            return value.Trim();
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> with Trim() applied.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static string OptionalString(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return OptionalString(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> with Trim() applied.
        /// </summary>
        public static string OptionalString(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Trim();
        }

        #endregion String

        #region URL

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> after validating that it is a formatted URL.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        public static string String_URL(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return String_URL(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a formatted URL.
        /// </summary>
        /// <param name="fieldName">The name of the value as it should display in any thrown exception messages.</param>
        public static string String_URL(IEnumerable<string> data, int index, string fieldName)
        {
            return String_URL(data?.ElementAtOrDefault(index), fieldName);
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
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> after validating that it is a formatted URL.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static string OptionalString_URL(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return OptionalString_URL(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a formatted URL.
        /// </summary>
        public static string OptionalString_URL(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString_URL(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a formatted URL.
        /// </summary>
        /// <exception cref="URLException"></exception>
        public static string OptionalString_URL(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            //Validate that this string is a URL
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                throw new URLException(fieldName, value);

            return value;
        }

        #endregion URL

        #region HexCode

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> after validating that it is a hex code.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static string String_HexCode(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return String_HexCode(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a hex code.
        /// </summary>
        public static string String_HexCode(IEnumerable<string> data, int index, string fieldName)
        {
            return String_HexCode(data?.ElementAtOrDefault(index), fieldName);
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
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="indices"/> after validating that it is a hex color code.
        /// </summary>
        /// <param name="indices">(int, int) is the (set, cell) location of the value in <paramref name="data"/>.</param>
        public static string OptionalString_HexCode(IEnumerable<IEnumerable<string>> data, (int, int) indices, string fieldName)
        {
            return OptionalString_HexCode(data?.ElementAtOrDefault(indices.Item1), indices.Item2, fieldName);
        }

        /// <summary>
        /// Returns the value of the cell in <paramref name="data"/> at <paramref name="index"/> after validating that it is a hex color code.
        /// </summary>
        public static string OptionalString_HexCode(IEnumerable<string> data, int index, string fieldName)
        {
            return OptionalString_HexCode(data?.ElementAtOrDefault(index), fieldName);
        }

        /// <summary>
        /// Returns <paramref name="value"/> after validating that it is a hex color code.
        /// </summary>
        /// <exception cref="HexException"></exception>
        public static string OptionalString_HexCode(string value, string fieldName)
        {
            value = OptionalString(value, fieldName);

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            //Validate that this string is a hex code
            Match match = hexRegex.Match(value);
            if (!match.Success)
                throw new HexException(fieldName, value);

            //Return hex formatted with a # symbol
            return $"#{match.Groups[1]}";
        }

        #endregion HexCode
    }
}
