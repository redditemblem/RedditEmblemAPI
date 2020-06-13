namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class PositiveIntegerException : IntegerException
    {
        public PositiveIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a positive numerical value")
        { }
    }
}
