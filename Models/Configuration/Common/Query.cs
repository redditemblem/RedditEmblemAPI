﻿using Newtonsoft.Json;
using System.Collections.Generic;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    /// <summary>
    /// Container class for deserialized JSON <c>"Query"</c> object data. Represents required fields for Google Sheets API queries.
    /// </summary>
    /// <see cref="https://developers.google.com/sheets/api/guides/concepts"/>
    public class Query
    {
        #region Required Fields

        /// <summary>
        /// The name of the Google Sheet to execute the query on.
        /// </summary>
        [JsonRequired]
        public string Sheet { get; set; }

        /// <summary>
        /// The range of cells to execute the query on. Must be in a <c>"StartCell:EndCell"</c> format.
        /// </summary>
        [JsonRequired]
        public string Selection { get; set; }

        /// <summary>
        /// The dimension that the data will be returned in. Valid values are:
        /// <list type="bullet">
        /// <item>
        /// <term>1</term>
        /// <description>Returns data organized by ROWS</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Returns data organized by COLUMNS</description>
        /// </item>
        /// </list>
        /// </summary>
        [JsonRequired]
        public MajorDimensionEnum Orientation { get; set; }

        #endregion

        #region Optional Fields

        /// <summary>
        /// Optional. Flag indicating whether or not this query should error if the data comes back null. Defaults to true.
        /// </summary>
        public bool AllowNullData { get; set; } = true;

        #endregion

        /// <summary>
        /// Not a JSON value. The matrix of objects returned from Google after execution of this query.
        /// </summary>
        [JsonIgnore]
        public IList<IList<object>> Data { get; set; } = new List<IList<object>>();

        /// <summary>
        /// Overridden. Returns the worksheet query in <c>Sheet!Selection</c> format for passing to the Google Sheets API.
        /// </summary>
        public override string ToString()
        {
            return $"{this.Sheet}!{this.Selection}";
        }
    }
}
