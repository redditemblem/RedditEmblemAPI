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
        }
    }
}
