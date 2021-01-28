using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;

namespace RedditEmblemAPI.Models.Configuration.Turns
{
    public class TurnConfig
    {
        #region Required Fields

        [JsonRequired]
        public Query Query { get; set; }

        #endregion

        #region Constants

        public readonly int TurnID = 0;

        public readonly int AmendedByTurnID = 1;

        public readonly int TurnOrder = 2;

        public readonly int UnitName = 3;

        public readonly int PlayerName = 4;

        public readonly int BeforeConditional = 5;

        public readonly int AfterConditional = 6;

        public readonly int Action = 7;

        public readonly int Notes = 8;

        public readonly int InCharacter = 9;

        public readonly int Processed = 10;

        #endregion
    }
}
