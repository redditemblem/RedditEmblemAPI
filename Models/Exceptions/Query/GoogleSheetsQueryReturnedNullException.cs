using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class GoogleSheetsQueryReturnedNullException : Exception
    {
        /// <summary>
        /// Thrown when a Google Sheets query returns no values.
        /// </summary>
        public GoogleSheetsQueryReturnedNullException(string sheetName)
            : base(string.Format("The \"{0}\" sheet query returned no data.", sheetName))
        {}
    }
}
