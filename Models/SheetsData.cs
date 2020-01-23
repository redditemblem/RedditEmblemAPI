using RedditEmblemAPI.Models.Exceptions;
using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models
{
    public class SheetsData
    {
        public List<Unit> Units;
        public List<Exception> Errors;

        public SheetsData()
        {
            this.Units = new List<Unit>();
            this.Errors = new List<Exception>();

            this.Units.Add(new Unit() { Name = "Sequoia", SpriteURL = "", Coordinates = new Coordinate { X = 3, Y = 4 } });
            this.Errors.Add(new PositiveIntegerException("Sheet!A1", "-3"));
        }
    }
}
