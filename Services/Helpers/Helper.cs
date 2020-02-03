using RedditEmblemAPI.Models.Exceptions;

namespace RedditEmblemAPI.Services.Helpers
{
    public abstract class Helper
    {
        protected static int SafeIntParse(string number, string fieldName, bool isPositive)
        {
            int val;
            if (!int.TryParse(number, out val))
            {
                if (isPositive) throw new PositiveIntegerException(fieldName, number);
                else throw new AnyIntegerException(fieldName, number);
            }
            else if (isPositive && val < 0)
                throw new PositiveIntegerException(fieldName, number);
            return val;
        }
    }
}
