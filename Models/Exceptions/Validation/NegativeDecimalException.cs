namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NegativeDecimalException : NumericalException
    {
        /// <summary>
        /// Thrown when a decimal fails to parse and the expected result is a number below 0.
        /// </summary>
        public NegativeDecimalException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a negative decimal")
        { }
    }
}
