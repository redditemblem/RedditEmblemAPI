using RedditEmblemAPI.Models.Configuration.Turns;
using RedditEmblemAPI.Services.Helpers;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Output.Turns
{
    public class Turn
    {
        public int TurnID { get; set; } = -1;

        public int AmendedByTurnID { get; set; } = -1;

        public IList<Turn> AmendedTurns { get; set; } = new List<Turn>();

        public int TurnOrder { get; set; }

        public string UnitName { get; set; }

        public string PlayerName { get; set; }

        public string BeforeConditional { get; set; } = string.Empty;

        public string AfterConditional { get; set; } = string.Empty;

        public string Action { get; set; }

        public string InCharacter { get; set; } = string.Empty;

        private string Processed { get; set; } = "No";

        public bool IsProcessed { get { return this.Processed == "Yes"; } }

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Turn() { }

        /// <summary>
        /// Constructor. Populates attributes with values from <paramref name="data"/>.
        /// </summary>
        public Turn(TurnConfig config, IList<string> data)
        {
            this.AmendedTurns = new List<Turn>();

            this.TurnID = ParseHelper.Int_NonZeroPositive(data, config.TurnID, "TurnID");
            this.AmendedByTurnID = ParseHelper.OptionalInt_NonZeroPositive(data, config.AmendedByTurnID, "AmendedByTurnID", -1);
            this.TurnOrder = ParseHelper.OptionalInt_NonZeroPositive(data, config.TurnOrder, "TurnOrder", -1);

            this.UnitName = ParseHelper.SafeStringParse(data, config.UnitName, "UnitName", true);
            this.PlayerName = ParseHelper.SafeStringParse(data, config.PlayerName, "PlayerName", true);
            this.BeforeConditional = ParseHelper.SafeStringParse(data, config.BeforeConditional, "BeforeConditional", false);
            this.AfterConditional = ParseHelper.SafeStringParse(data, config.AfterConditional, "AfterConditional", false);
            this.Action = ParseHelper.SafeStringParse(data, config.Action, "Action", false);
            this.InCharacter = ParseHelper.SafeStringParse(data, config.InCharacter, "InCharacter", false);
            this.Processed = ParseHelper.SafeStringParse(data, config.Processed, "Processed", false);
        }

        #endregion

        public IList<IList<object>> ToDataMatrix()
        {
            IList<IList<object>> contents = new List<IList<object>>();

            //These items should be in the same order as the TurnConfig consts
            contents.Add(new List<object>(){
                this.TurnID.ToString(),
                (this.AmendedByTurnID == -1 ? string.Empty : this.AmendedByTurnID.ToString()),
                (this.TurnOrder == -1 ? string.Empty : this.TurnOrder.ToString()),
                this.UnitName,
                this.PlayerName,
                this.BeforeConditional,
                this.AfterConditional,
                this.Action,
                this.InCharacter,
                this.Processed
            });

            return contents;
        }
    }
}
