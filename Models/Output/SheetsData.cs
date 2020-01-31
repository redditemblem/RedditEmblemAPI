using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class SheetsData
    {
        public IList<Unit> Units;
        public IList<Exception> Errors;

        public SheetsData()
        {
            this.Units = new List<Unit>();
            this.Errors = new List<Exception>();
        }
    }
}
