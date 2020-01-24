using RedditEmblemAPI.Models;
using RedditEmblemAPI.Models.Output;
using System.Collections.Generic;

namespace RedditEmblemAPI.Services
{
    public interface IAPIService
    {
        SheetsData LoadData(string teamName);
        List<string> LoadTeamList();
    }
}