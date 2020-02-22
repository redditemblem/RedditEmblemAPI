using RedditEmblemAPI.Models.Exceptions;
using System;

namespace RedditEmblemAPI.Models.Common
{
    /// <summary>
    /// Struct representing a coordinate pair on the map.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// The coordinate's horizontal displacement value.
        /// </summary>
        public int X;

        /// <summary>
        /// The coordinate's vertical displacement value.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the passed in <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the passed in <paramref name="coord"/> value in "x,y" format.
        /// If <paramref name="coord"/> is an empty string instead, sets both <c>X</c> and <c>Y</c> to 0.
        /// </summary>
        /// <param name="coord"></param>
        /// <exception cref="CoordinateFormattingException"></exception>
        public Coordinate(string coord)
        {
            if(string.IsNullOrEmpty(coord))
            {
                this.X = 0;
                this.Y = 0;
                return;
            }

            //Error if the passed string is not a tuple of non-zero, positive integers
            string[] split = coord.Split(',');
            if (   split.Length != 2 
                || !int.TryParse(split[0].Trim(), out this.X)
                || !int.TryParse(split[1].Trim(), out this.Y)
                || this.X < 1
                || this.Y < 1
               )
                throw new CoordinateFormattingException(coord);
        }

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the same x,y values as <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord"></param>
        public Coordinate(Coordinate coord)
        {
            this.X = coord.X;
            this.Y = coord.Y;
        }

        public override bool Equals(Object obj)
        {
            return Equals((Coordinate)obj);
        }

        public bool Equals(Coordinate obj)
        {
            return this.X == obj.X && this.Y == obj.Y;
        }
    }
}