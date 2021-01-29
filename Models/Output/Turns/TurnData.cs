using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditEmblemAPI.Models.Output.Turns
{
    public class TurnData
    {
        public IList<Turn> SubmittedTurns { get; private set; }

        public TurnData(JSONConfiguration config)
        {
            this.SubmittedTurns = new List<Turn>();
            foreach (IList<object> row in config.Turns.Query.Data)
            {
                try
                {
                    IList<string> turn = row.Select(r => r.ToString()).ToList();
                    string turnID = ParseHelper.SafeStringParse(turn, config.Turns.TurnID, "TurnID", false);
                    if (string.IsNullOrEmpty(turnID)) continue;
                    this.SubmittedTurns.Add(new Turn(config.Turns, turn));
                }
                catch (Exception ex)
                {
                    throw new Exception("ERROR: " + (row.ElementAtOrDefault(config.Turns.TurnID) ?? string.Empty).ToString(), ex);
                }
            }

            //Create history stacks
            foreach (Turn turn in this.SubmittedTurns.Where(t => t.AmendedByTurnID == -1))
            {
                IList<Turn> amendedList = new List<Turn>();
                BuildAmendedTurnHistory(turn, amendedList);

                turn.AmendedTurns = amendedList;
            }

            //Remove all the amended turns
            while (this.SubmittedTurns.Any(t => t.AmendedByTurnID != -1))
            {
                this.SubmittedTurns.Remove(this.SubmittedTurns.First(t => t.AmendedByTurnID != -1));
            }
        }

        private void BuildAmendedTurnHistory(Turn turn, IList<Turn> amendedList)
        {
            Turn amended = this.SubmittedTurns.FirstOrDefault(t => t.AmendedByTurnID == turn.TurnID);
            if (amended == null)
                return;

            amendedList.Add(amended); //Add turn to the history stack
            BuildAmendedTurnHistory(amended, amendedList);
        }
    }
}
