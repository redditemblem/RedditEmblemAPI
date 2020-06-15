using System;

namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedTileEffectException : Exception
    {
        public UnmatchedTileEffectException(int x, int y, string effectName)
            : base(string.Format("The terrain effect \"{0}\" located at coordinate \"({1},{2})\" could not be matched to a known terrain effect definition. The given name must match exactly, including capitalization and punctuation.",
                   effectName, x, y
                  ))
        { }
    }
}
