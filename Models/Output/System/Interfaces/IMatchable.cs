namespace RedditEmblemAPI.Models.Output.System.Interfaces
{
    /// <summary>
    /// Requires class to include a <c>Matched</c> flag.
    /// </summary>
    public interface IMatchable
    {
        bool Matched { get; }
    }
}
