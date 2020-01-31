using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest;

namespace RedditEmblemAPI.Models.Configuration.Common
{
    public class WorksheetQuery
    {
        public string Sheet;
        public string Selection;

        // ROWS = 1, COLUMNS = 2
        public MajorDimensionEnum Orientation;
        

        public override string ToString()
        {
            return string.Format("{0}!{1}", this.Sheet, this.Selection);
        }
    }
}
