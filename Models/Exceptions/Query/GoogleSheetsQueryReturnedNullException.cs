using System;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class GoogleSheetsQueryReturnedNullException : Exception
    {
        public GoogleSheetsQueryReturnedNullException()
            : base("The sheet query returned no data.")
        {}
    }
}
