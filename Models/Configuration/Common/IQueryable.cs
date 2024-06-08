using System.Collections.Generic;

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

    /// <summary>
    /// Requires class to include a <c>Queries</c> list.
    /// </summary>
    public interface IMultiQueryable
    {
        /// <summary>
        /// [IMultiQueryable] Represents the Google Sheets API queries associated with data for this class.
        /// </summary>
        public IEnumerable<Query> Queries { get; }
    }
}
