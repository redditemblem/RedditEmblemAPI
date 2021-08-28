namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTerrainEffectLayerException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of an <c>TerrainEffectLayer</c>.
        /// </summary>
        /// <param name="terrainEffectLayer"></param>
        public UnmatchedTerrainEffectLayerException(string terrainEffectLayer)
            : base("terrain effect layer", terrainEffectLayer)
        { }
    }
}
