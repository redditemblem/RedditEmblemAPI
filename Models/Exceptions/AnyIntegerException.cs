namespace RedditEmblemAPI.Models.Exceptions
{
    public class AnyIntegerException : IntegerException
    {
        public AnyIntegerException(string cell, string actualValue)
            : base(cell, actualValue, "a numerical value")
        { }
    }
}