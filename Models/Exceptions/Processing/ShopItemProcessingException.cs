using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ShopItemProcessingException : Exception
    {
        public ShopItemProcessingException(string itemName, Exception innerException)
            : base(string.Format("An error occurred while processing shop item \"{0}\".", itemName), innerException)
        { }
    }
}
