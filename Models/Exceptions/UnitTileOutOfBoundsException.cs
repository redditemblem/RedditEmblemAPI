using RedditEmblemAPI.Models.Common;
using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class UnitTileOutOfBoundsException : Exception
    {
        public UnitTileOutOfBoundsException(Coordinate coord)
            : base(string.Format("Attempted to access map tile \"({0},{1})\" but it was not found. Does this unit overlap a map boundary?", coord.X, coord.Y))
        { }

        public UnitTileOutOfBoundsException(int x, int y)
            : base(string.Format("Attempted to access map tile \"({0},{1})\" but it was not found. Does this unit overlap a map boundary?", x, y))
        { }
    }
}
