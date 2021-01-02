using RedditEmblemAPI.Models.Exceptions.Validation;
using System;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// Struct representing a coordinate pair on the map.
    /// </summary>
    public class Coordinate
    {
        #region Attributes

        /// <summary>
        /// The coordinate's horizontal displacement value.
        /// </summary>
        public int X;

        /// <summary>
        /// The coordinate's vertical displacement value.
        /// </summary>
        public int Y;

        #endregion

        #region Constructors

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

        #endregion

        /// <summary>
        /// Returns the Manhattan Distance (horizontal/vertical displacement) between this coordinate and <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public int DistanceFrom(Coordinate coord)
        {
            return DistanceFrom(coord.X, coord.Y);
        }

        /// <summary>
        /// Returns the Manhattan Distance (horizontal/vertical displacement) between this coordinate and the coordinate at <paramref name="x"/>, <paramref name="y"/>.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public int DistanceFrom(int x, int y)
        {
            return Math.Abs(this.X - x) + Math.Abs(this.Y - y);
        }

        /// <summary>
        /// Returns the <c>Coordinate</c> in "x,y" format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.X + "," + this.Y;
        }

        #region Equivalence Functions

        public override bool Equals(Object obj)
        {
            return Equals((Coordinate)obj);
        }

        /// <summary>
        /// Returns true if this coordinate and <paramref name="coord"/> possess the same <c>X</c> and <c>Y</c> values.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public bool Equals(Coordinate coord)
        {
            return this.X == coord.X && this.Y == coord.Y;
        }

        #endregion
    }
}