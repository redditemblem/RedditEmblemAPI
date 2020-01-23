using RedditEmblemAPI.Models;

namespace RedditEmblemAPI.Services
{
    public class SheetsService : ISheetsService
    {
        public SheetsData SheetData;

        public SheetsData LoadData()
        {
            this.SheetData = new SheetsData();
            return this.SheetData;
        }
    }
}
