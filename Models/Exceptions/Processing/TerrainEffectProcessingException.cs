using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TerrainEffectProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>TerrainEffect</c>.
        /// </summary>
        /// <param name="terrainEffectName"></param>
        /// <param name="innerException"></param>
        public TerrainEffectProcessingException(string terrainEffectName, Exception innerException)
            : base("terrain effect", terrainEffectName, innerException)
        { }
    }
}
