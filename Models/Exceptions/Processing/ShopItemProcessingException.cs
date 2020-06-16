using System;

namespace RedditEmblemAPI.Models.Exceptions.Processing
{
    public class ShopItemProcessingException : ProcessingException
    {
        /// <summary>
        /// Container exception thrown when an error occurs while processing a <c>ShopItem</c>.
        /// </summary>
        /// <param name="shopItemName"></param>
        /// <param name="innerException"></param>
        public ShopItemProcessingException(string shopItemName, Exception innerException)
            : base("shop item", shopItemName, innerException)
        { }
    }
}
