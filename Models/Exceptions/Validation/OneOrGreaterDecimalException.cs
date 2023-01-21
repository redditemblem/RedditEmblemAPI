namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class OneOrGreaterDecimalException : NumericalException
    {
        /// <summary>
        /// Thrown when a decimal fails to parse and the expected result is a numerical value greater than or equal to 1.
        /// </summary>
        public OneOrGreaterDecimalException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a decimal value greater than or equal to 1")
        { }
    }
}
