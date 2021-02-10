using RedditEmblemAPI.Models.Output.Turns;
using System.Collections.Generic;

namespace RedditEmblemAPI.Models.Input.Turns
{
    public class ClientTurnData
    {
        public Turn NewTurn { get; set; }

        public IList<Turn> Turns { get; set; }
    }
}
