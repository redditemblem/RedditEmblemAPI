using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds the <c>Query</c> object attribute from <paramref name="queryable"/>, if it exists, to calling List<Query> obj. Returns true if the add was successful.
        /// </summary>
        public static bool AddQueryable(this List<Query> queries, IQueryable queryable)
        {
            if (queryable?.Query == null)
                return false;

            queries.Add(queryable.Query);
            return true;
        }

        /// <summary>
        /// Adds the contents of the <c>Queries</c> object attribute from <paramref name="queryable"/>, if it exists, to calling List<Query> obj. Returns true if the add was successful.
        /// </summary>
        public static bool AddQueryable(this List<Query> queries, IMultiQueryable queryable)
        {
            if (queryable == null || queryable.Queries == null || !queryable.Queries.Any())
                return false;

            queries.AddRange(queryable.Queries);
            return true;
        }
    }
}
