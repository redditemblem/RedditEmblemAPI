namespace RedditEmblemAPI.Models.Configuration.Common
{
    public static class IntTupleExtensionMethods
    {
        /// <summary>
        /// Returns true if both of <paramref name="tuple"/>'s integer values are >= 0.
        /// </summary>
        public static bool IsConfigured(this (int, int) tuple)
        {
            return tuple.Item1 >= 0 && tuple.Item2 >= 0;
        }
    }
}
