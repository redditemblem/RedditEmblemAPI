using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TerrainTypeProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>TerrainType</c>.
        /// </summary>
        /// <param name="terrainTypeName"></param>
        /// <param name="innerException"></param>
        public TerrainTypeProcessingException(string terrainTypeName, Exception innerException)
            : base("terrain type", terrainTypeName, innerException)
        { }
    }
}
