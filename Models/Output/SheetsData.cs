using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class SheetsData
    {
        public List<Unit> Units;
        public List<Exception> Errors;

        public SheetsData()
        {
            this.Units = new List<Unit>();
            this.Errors = new List<Exception>();
        }
    }
}
