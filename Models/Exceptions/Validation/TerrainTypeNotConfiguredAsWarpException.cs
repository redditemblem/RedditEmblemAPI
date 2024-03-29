﻿using System;

namespace RedditEmblemAPI.Models.Exceptions.Validation
{
    public class TerrainTypeNotConfiguredAsWarpException : Exception
    {
        /// <summary>
        /// Thrown when a tile has been placed into a warp group but its terrain type is not configured as a warp.
        /// </summary>
        /// <param name="terrainTypeName"></param>
        /// <param name="tileValue"></param>
        public TerrainTypeNotConfiguredAsWarpException(string terrainTypeName, string tileValue)
            : base($"The terrain type \"{terrainTypeName}\" found in tile \"{tileValue}\" has not been configured with a warp type. Terrain types must have a warp type to be used in a warp group.")
        { }
    }
}
