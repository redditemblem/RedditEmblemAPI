﻿using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Units;
using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class UnitTileOverlapException : Exception
    {
        /// <summary>
        /// Thrown when more than one unit exists on a map tile.
        /// </summary>
        /// <param name="overlappingUnit">The unit attempting to be placed on the tile.</param>
        /// <param name="existingUnit">The unit already on the tile.</param>
        /// <param name="coord">The location where the conflict is occurring.</param>
        public UnitTileOverlapException(Unit overlappingUnit, Unit existingUnit, Coordinate coord)
            : base(string.Format("Unit \"{0}\" cannot be placed overlapping unit \"{1}\" at map tile \"{2},{3}\".", overlappingUnit.Name, existingUnit.Name, coord.X, coord.Y))
        { }
    }
}
