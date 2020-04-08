using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class GoogleSheetsQueryFailedException : Exception
    {
        public GoogleSheetsQueryFailedException(string sheets, Exception innerException)
            : base(string.Format("There was an error querying a sheet in the set of \"{0}\" using the Google Sheets API.", sheets), innerException)
        { }
    }
}
