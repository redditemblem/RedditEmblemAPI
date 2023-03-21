using System.Collections.Generic;

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
    }
}
