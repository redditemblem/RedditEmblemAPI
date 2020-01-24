using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class GoogleSheetsAccessDeniedException : Exception
    {
        public GoogleSheetsAccessDeniedException()
            : base("Access was denied to the Google Sheets API.")
        { }
    }
}
