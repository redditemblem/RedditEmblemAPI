using RedditEmblemAPI.Models.Input.Turns;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Storage.Convoy;
using RedditEmblemAPI.Models.Output.Storage.Shop;
using RedditEmblemAPI.Models.Output.Teams;
using RedditEmblemAPI.Models.Output.Turns;
using System.Collections.Generic;

namespace RedditEmblemAPI.Services
{
    public interface IAPIService
    {
        MapData LoadMapData(string teamName);
        TurnData LoadMapTurnData(string teamName);
        void CreateTeamMapTurn(string teamName, ClientTurnData postData);
        ConvoyData LoadConvoyData(string teamName);
        ShopData LoadShopData(string teamName);
        IList<TeamData> LoadTeamList();
    }
}