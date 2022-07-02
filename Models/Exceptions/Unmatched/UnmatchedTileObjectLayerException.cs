namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileObjectLayerException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>TileObjectLayer</c>.
        /// </summary>
        public UnmatchedTileObjectLayerException(string tileObjectLayer)
            : base("tile object layer", tileObjectLayer)
        { }
    }
}
