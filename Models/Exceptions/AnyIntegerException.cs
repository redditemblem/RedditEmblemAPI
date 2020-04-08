namespace RedditEmblemAPI.Models.Exceptions
{
    public class AnyIntegerException : IntegerException
    {
        public AnyIntegerException(string fieldName, string actualValue)
            : base(fieldName, actualValue, "a numerical value")
        { }
    }
}