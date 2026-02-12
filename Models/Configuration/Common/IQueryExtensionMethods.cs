using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    public static class IQueryExtensionMethods
    {
        /// <summary>
        /// Adds the query from <paramref name="queryable"/>, if it exists, to the calling <paramref name="queries"/> list. Returns true if the add was successful.
        /// </summary>
        public static bool AddQueryable(this List<IQuery> queries, IQueryable queryable)
        {
            if (queryable?.Query is null)
                return false;

            queries.Add(queryable.Query);
            return true;
        }

        /// <summary>
        /// Adds the range of queries from <paramref name="multiQueryable"/>, if any exist, to the calling <paramref name="queries"/> list. Returns true if the add was successful.
        /// </summary>
        public static bool AddQueryable(this List<IQuery> queries, IMultiQueryable multiQueryable)
        {
            if (multiQueryable?.Queries is null || !multiQueryable.Queries.Any())
                return false;

            queries.AddRange(multiQueryable.Queries);
            return true;
        }
    }
}
