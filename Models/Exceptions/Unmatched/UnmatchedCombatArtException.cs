namespace RedditEmblemAPI.Models.Exceptions.Unmatched
{
    public class UnmatchedCombatArtException : UnmatchedException
    {
        /// <summary>
        /// Thrown when text cannot be matched to the name of a <c>CombatArt</c>.
        /// </summary>
        public UnmatchedCombatArtException(string combatArtName)
            : base("combat art", combatArtName)
        { }
    }
}
