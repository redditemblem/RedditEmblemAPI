namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NonZeroPositiveIntegerException : IntegerException
    {
        /// <summary>
        /// Thrown when an integer fails to parse and the expected result is a non-zero positive number.
        /// </summary>
        public NonZeroPositiveIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a non-zero, positive integer")
        { }
    }
}
