using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class GoogleSheetsQueryFailedException : Exception
    {
        /// <summary>
        /// Container exception thrown when any error occurs quering Google Sheets.
        /// </summary>
        /// <param name="sheetNamesCSV"></param>
        /// <param name="innerException"></param>
        public GoogleSheetsQueryFailedException(string sheetNamesCSV, Exception innerException)
            : base(string.Format("There was an error querying a sheet in the set of \"{0}\" using the Google Sheets API.", sheetNamesCSV), innerException)
        { }
    }
}
