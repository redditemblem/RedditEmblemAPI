using RedditEmblemAPI.Models.Configuration.Turns;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Turns
{
    public class Turn
    {
        public int TurnID { get; private set; }

        public int AmendedByTurnID { get; private set; }

        public int TurnOrder { get; private set; }

        public string UnitName { get; private set; }

        public string PlayerName { get; private set; }

        public string BeforeConditional { get; private set; }

        public string AfterConditional { get; private set; }

        public string Action { get; private set; }

        public string Notes { get; private set; }

        public string InCharacter { get; private set; }

        private string Processed { get; set; }

        public bool IsProcessed { get { return this.Processed == "Yes"; } }

        public Turn(TurnConfig config, IList<string> data)
        {
            this.TurnID = ParseHelper.SafeIntParse(data, config.TurnID, "TurnID", true, true);
            this.AmendedByTurnID = ParseHelper.OptionalSafeIntParse(data, config.AmendedByTurnID, "AmendedByTurnID", true, true, -1);
            this.TurnOrder = ParseHelper.SafeIntParse(data, config.TurnOrder, "TurnOrder", true, true);

            this.UnitName = ParseHelper.SafeStringParse(data, config.UnitName, "UnitName", true);
            this.PlayerName = ParseHelper.SafeStringParse(data, config.PlayerName, "PlayerName", true);
            this.BeforeConditional = ParseHelper.SafeStringParse(data, config.BeforeConditional, "BeforeConditional", false);
            this.AfterConditional = ParseHelper.SafeStringParse(data, config.AfterConditional, "AfterConditional", false);
            this.Action = ParseHelper.SafeStringParse(data, config.Action, "Action", false);
            this.Notes = ParseHelper.SafeStringParse(data, config.Notes, "Notes", false);
            this.InCharacter = ParseHelper.SafeStringParse(data, config.InCharacter, "InCharacter", false);
            this.Processed = "No";
        }
    }
}
