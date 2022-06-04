namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NegativeIntegerException : NumericalException
    {
        /// <summary>
        /// Thrown when an integer fails to parse and the expected result is a number below 0.
        /// </summary>
        public NegativeIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a negative integer")
        { }
    }
}
