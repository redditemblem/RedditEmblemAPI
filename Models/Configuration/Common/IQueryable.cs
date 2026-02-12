using Newtonsoft.Json;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    #region IQueryable

    /// <summary>
    /// Requires class to include a <c>Query</c> object.
    /// </summary>
    public interface IQueryable
    {
        /// <inheritdoc cref="Queryable.Query"/>
        public Query Query { get; set; }
    }

    /// <summary>
    /// Implements a <c>Query</c> object.
    /// </summary>
    public abstract class Queryable : IQueryable
    {
        /// <summary>
        /// [IQueryable] The Google Sheets API query associated with this object.
        /// </summary>
        [JsonRequired]
        public Query Query { get; set; }
    }

    #endregion IQueryable

    #region IMultiQueryable

    /// <summary>
    /// Requires class to include a <c>Queries</c> list and <c>Name</c> identifier.
    /// </summary>
    public interface IMultiQueryable
    {
        /// <inheritdoc cref="MultiQueryable.Queries"/>
        public IEnumerable<Query> Queries { get; set; }

        /// <inheritdoc cref="MultiQueryable.Name"/>
        int Name { get; set; }
    }

    /// <summary>
    /// Implements a <c>Queries</c> list and <c>Name</c> identifier.
    /// </summary>
    public abstract class MultiQueryable : IMultiQueryable
    {
        /// <summary>
        /// [IMultiQueryable] The set of Google Sheets API queries associated with this object.
        /// </summary>
        [JsonRequired]
        public IEnumerable<Query> Queries { get; set; }

        /// <summary>
        /// [IMultiQueryable] Required. Cell index for this object's name.
        /// </summary>
        [JsonRequired]
        public int Name { get; set; }
    }

    #endregion IMultiQueryable
}
