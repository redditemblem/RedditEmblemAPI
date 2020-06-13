using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Exceptions.Query
{
    public class ShopNotConfiguredException : Exception
    {
        public ShopNotConfiguredException()
            : base("Shop functionality has not been setup for this team.")
        { }
    }
}
