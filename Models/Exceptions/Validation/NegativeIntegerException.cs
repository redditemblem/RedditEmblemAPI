namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class NegativeIntegerException : IntegerException
    {
        public NegativeIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a non-negative numerical value")
        { }
    }
}