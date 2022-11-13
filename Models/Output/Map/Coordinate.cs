using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Validation;
using System;
using System.Text.RegularExpressions;

namespace RedditEmblemAPI.Models.Output.Map
{
    /// <summary>
    /// A coordinate pair (ex. "x,y") on the map.
    /// </summary>
    public class Coordinate
    {
        #region Constants

        private readonly Regex alphanumericRegex = new Regex("^([A-Z]+)([0-9]+)$");

        #endregion Constants

        #region Attributes

        /// <summary>
        /// The coordinate's horizontal displacement value.
        /// </summary>
        public int X;

        /// <summary>
        /// The coordinate's vertical displacement value.
        /// </summary>
        public int Y;

        /// <summary>
        /// The textual representation of the coordinate. Can be in x,y or alphanumerical representation.
        /// </summary>
        public string AsText;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <c>Coordinate with 0,0 for the X and Y values.</c>
        /// </summary>
        public Coordinate()
        {
            this.X = 0;
            this.Y = 0;
            this.AsText = string.Empty;
        }

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the passed in <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Coordinate(CoordinateFormat coordinateFormat, int x, int y)
        {
            this.X = x;
            this.Y = y;
            SetAsTextValue(coordinateFormat);
        }

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the passed in <paramref name="coord"/> value.
        /// If <paramref name="coord"/> is an empty string instead, sets both <c>X</c> and <c>Y</c> to 0.
        /// </summary>
        /// <exception cref="XYCoordinateFormattingException"></exception>
        /// <exception cref="AlphanumericCoordinateFormattingException"></exception>
        public Coordinate(CoordinateFormat coordinateFormat, string coord)
        {
            if (string.IsNullOrEmpty(coord))
            {
                this.X = 0;
                this.Y = 0;
                this.AsText = string.Empty;
                return;
            }

            if (coordinateFormat == CoordinateFormat.XY)
            {
                //Error if the passed string is not a tuple of non-zero, positive integers
                string[] split = coord.Split(',');
                if (split.Length != 2
                    || !int.TryParse(split[0].Trim(), out this.X)
                    || !int.TryParse(split[1].Trim(), out this.Y)
                    || this.X < 1
                    || this.Y < 1
                   )
                    throw new XYCoordinateFormattingException(coord);
                this.AsText = coord;
            }
            else
            {
                coord = coord.ToUpper();
                Match match = alphanumericRegex.Match(coord);
                if (!match.Success)
                    throw new AlphanumericCoordinateFormattingException(coord);

                this.X = 0;
                string alpha = match.Groups[1].Value;
                for (int i = 0; i < alpha.Length; i++)
                {
                    int letter = (int)alpha[alpha.Length - i - 1] - 64;
                    this.X += letter * (int)Math.Pow(26, i);
                }

                this.Y = int.Parse(match.Groups[2].Value);
                this.AsText = coord;
            }
        }

        /// <summary>
        /// Initializes the <c>Coordinate</c> with the same x,y values as <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord"></param>
        public Coordinate(Coordinate coord)
        {
            this.X = coord.X;
            this.Y = coord.Y;
            this.AsText = coord.AsText;
        }

        #endregion

        private void SetAsTextValue(CoordinateFormat coordinateFormat)
        {
            switch (coordinateFormat)
            {
                case CoordinateFormat.XY: this.AsText = BuildAsTextValue_XY(); break;
                case CoordinateFormat.Alphanumerical: this.AsText = BuildAsTextValue_Alphanumerical(); break;
                default: throw new Exception("Unrecognized coordinate format");
            }
        }

        /// <summary>
        /// Returns the values in <c>this.X</c> and <c>this.Y</c> as a string formatted in "x,y" notation.
        /// </summary>
        private string BuildAsTextValue_XY()
        {
            if (this.X < 1 || this.Y < 1)
                return string.Empty;

            return $"{this.X},{this.Y}";
        }

        /// <summary>
        /// Returns the values in <c>this.X</c> and <c>this.Y</c> as a string formatted in alphanumerical notation (ex. "A1").
        /// </summary>
        private string BuildAsTextValue_Alphanumerical()
        {
            string alpha = string.Empty;
            if (this.X < 1 || this.Y < 1)
                return alpha;

            int x = this.X;
            do
            {
                int mod = (x - 1) % 26;
                alpha = Convert.ToChar(65 + mod) + alpha;
                x = (x - mod) / 26;
            } while (x > 0);

            alpha += this.Y.ToString();
            return alpha;
        }

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
        /// Returns the <c>Coordinate</c> in "x,y" or alphanumerical print format for display.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.AsText;
        }

        #region Equivalence Functions

        public override bool Equals(object obj)
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