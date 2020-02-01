using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class GoogleSheetsQueryFailedException : Exception
    {
        public GoogleSheetsQueryFailedException(string sheet, Exception innerException)
            : base(string.Format("There was an error querying the \"{0}\" sheet using the Google Sheets API.", sheet), innerException)
        { }
    }
}
