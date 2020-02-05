using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    public class SheetsData
    {
        public SheetsData()
        {
            this.Units = new List<Unit>();
            this.Errors = new List<Exception>();
        }

        public Map Map { get; set; }
        public IList<Unit> Units { get; set; }
        public IList<Exception> Errors { get; set; }
       
    }
}
