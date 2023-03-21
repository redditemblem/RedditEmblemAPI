namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Requires class to include a <c>Query</c> object.
    /// </summary>
    public interface IQueryable
    {
        public Query Query { get; }
    }
}
