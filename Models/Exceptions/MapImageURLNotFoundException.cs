using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class MapImageURLNotFoundException : Exception
    {
        public MapImageURLNotFoundException(string sheetName)
            : base(string.Format("The map image URL located on sheet \"{0}\" was found to be blank.", sheetName))
        { }
    }
}
