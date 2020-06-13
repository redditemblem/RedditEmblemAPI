namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class AnyIntegerException : IntegerException
    {
        public AnyIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a numerical value")
        { }
    }
}