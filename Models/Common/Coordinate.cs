using RedditEmblemAPI.Models.Exceptions;
using System;

namespace RedditEmblemAPI.Models.Common
{
    public struct Coordinate
    {
        public int X;
        public int Y;
        public bool IsHidden { get { return this.X < 0 || this.Y < 0; } }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coordinate(string coord)
        {
            if(string.IsNullOrEmpty(coord))
            {
                this.X = -1;
                this.Y = -1;
                return;
            }

            string[] split = coord.Split(',');
            if (split.Length != 2 || !int.TryParse(split[0].Trim(), out this.X) || !int.TryParse(split[1].Trim(), out this.Y))
                throw new CoordinateFormattingException(coord);
        }
    }
}