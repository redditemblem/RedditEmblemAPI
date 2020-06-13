using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Models.Output.Convoy;
using RedditEmblemAPI.Models.Output.Shop;
using RedditEmblemAPI.Models.Output.Teams;
using System.Collections.Generic;

namespace RedditEmblemAPI.Services
{
    public interface IAPIService
    {
        MapData LoadMapData(string teamName);

        ConvoyData LoadConvoyData(string teamName);

        ShopData LoadShopData(string teamName);

        IList<TeamData> LoadTeamList();
    }
}