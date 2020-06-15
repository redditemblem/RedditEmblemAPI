using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class TerrainEffectProcessingException : Exception
    {
        public TerrainEffectProcessingException(string effectName, Exception innerException)
            : base(string.Format("An error occurred while processing terrain effect \"{0}\".", effectName), innerException)
        { }
    }
}
