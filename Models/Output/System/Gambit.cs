﻿using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Battalions;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.System
{
    /// <summary>
    /// Object representing a gambit definition in the team's system. 
    /// </summary>
    public class Gambit
    {
        #region Attributes

        /// <summary>
        /// Flag indicating whether or not this gambit was found on a unit. Used to minify the output JSON.
        /// </summary>
        [JsonIgnore]
        public bool Matched { get; set; }

        /// <summary>
        /// The gambit's name. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The battalion's icon sprite URL.
        /// </summary>
        public string SpriteURL { get; set; }

        /// <summary>
        /// The gambit's max number of uses.
        /// </summary>
        public int MaxUses { get; set; }

        /// <summary>
        /// The unit stats (ex. Str/Mag/etc) that the gambit uses.
        /// </summary>
        public List<string> UtilizedStats { get; set; }

        /// <summary>
        /// Container object for the gambit's range values.
        /// </summary>
        public GambitRange Range { get; set; }

        /// <summary>
        /// Collection of stat values for the gambit. (ex. Hit)
        /// </summary>
        public IDictionary<string, int> Stats { get; set; }

        /// <summary>
        /// Any text information about the gambit to display.
        /// </summary>
        public List<string> TextFields { get; set; }

        #endregion

        public Gambit(GambitsConfig config, List<string> data)
        {
            this.Matched = false;
            this.Name = DataParser.String(data, config.Name, "Name");
            this.SpriteURL = DataParser.OptionalString_URL(data, config.SpriteURL, "Sprite URL");
            this.MaxUses = DataParser.Int_Positive(data, config.MaxUses, "Max Uses");
            this.UtilizedStats = DataParser.List_StringCSV(data, config.UtilizedStats);
            this.TextFields = DataParser.List_Strings(data, config.TextFields);

            this.Range = new GambitRange(config.Range, data);

            BuildStats(config.Stats, data);
        }

        private void BuildStats(List<NamedStatConfig> configs, List<string> data)
        {
            this.Stats = new Dictionary<string, int>();
            foreach (NamedStatConfig stat in configs)
            {
                int val = DataParser.Int_Any(data, stat.Value, stat.SourceName);
                this.Stats.Add(stat.SourceName, val);
            }
        }


        #region Static Functions

        public static IDictionary<string, Gambit> BuildDictionary(GambitsConfig config)
        {
            IDictionary<string, Gambit> gambits = new Dictionary<string, Gambit>();

            foreach (List<object> row in config.Query.Data)
            {
                try
                {
                    List<string> gambit = row.Select(r => r.ToString()).ToList();
                    string name = DataParser.OptionalString(gambit, config.Name, "Name");
                    if (string.IsNullOrEmpty(name)) continue;

                    if (!gambits.TryAdd(name, new Gambit(config, gambit)))
                        throw new NonUniqueObjectNameException("gambit");
                }
                catch (Exception ex)
                {
                    throw new GambitProcessingException((row.ElementAtOrDefault(config.Name) ?? string.Empty).ToString(), ex);
                }
            }

            return gambits;
        }

        #endregion

    }
}