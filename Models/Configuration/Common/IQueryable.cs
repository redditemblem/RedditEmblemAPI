namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Requires class to include a <c>Query</c> object.
    /// </summary>
    public interface IQueryable
    {
        /// <summary>
        /// [IQueryable] Represents the Google Sheets API query associated with data for this class.
        /// </summary>
        public Query Query { get; }
    }
}
