using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Storage.Convoy;
using RedditEmblemAPI.Models.Output.Storage.Shop;
using RedditEmblemAPI.Models.Output.Teams;
using System.Collections.Generic;

namespace RedditEmblemAPI.Services
{
    public interface IAPIService
    {
        MapData LoadMapData(string teamName);

        MapData LoadMapAnalysis(string teamName);

        byte[] GenerateMapImage(string teamName);

        ConvoyData LoadConvoyData(string teamName);

        ShopData LoadShopData(string teamName);

        IList<TeamData> LoadTeamList();
    }
}