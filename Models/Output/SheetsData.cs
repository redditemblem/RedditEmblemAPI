﻿using System;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output
{
    /// <summary>
    /// Wrapper class for the serialized JSON object data.
    /// </summary>
    public class SheetsData
    {
        /// <summary>
        /// Container object for data about the map.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Container list for data about units.
        /// </summary>
        public IList<Unit> Units { get; set; }

        /// <summary>
        /// Container list for soft exceptions that occurred during processing.
        /// </summary>
        public IList<Exception> Errors { get; set; }
       
    }
}
