using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class GoogleSheetsQueryFailedException : Exception
    {
        public GoogleSheetsQueryFailedException(Exception innerException)
            : base("There was an error querying the Google Sheets API.", innerException)
        { }
    }
}
