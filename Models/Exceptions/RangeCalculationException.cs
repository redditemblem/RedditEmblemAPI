using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class RangeCalculationException : Exception
    {
        public RangeCalculationException(string unitName, Exception innerException) 
            : base(string.Format("An error occurred while calculating map ranges for unit \"{0}\".", unitName), innerException)
        { }
    }
}
