namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NonZeroPositiveDecimalException : NumericalException
    {
        /// <summary>
        /// Thrown when a decimal fails to parse and the expected result is a non-zero positive number.
        /// </summary>
        public NonZeroPositiveDecimalException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a non-zero, positive decimal")
        { }
    }
}
