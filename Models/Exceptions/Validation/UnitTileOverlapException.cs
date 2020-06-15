using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitTileOverlapException : Exception
    {
        public UnitTileOverlapException(string overlappingUnitName, string existingUnitName, int x, int y)
            : base(string.Format("Unit \"{0}\" cannot be placed overlapping unit \"{1}\" at map tile \"({2},{3})\".", overlappingUnitName, existingUnitName, x, y))
        { }
    }
}
