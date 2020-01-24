namespace RedditEmblemAPI.Models.Configuration
{
    public class WorksheetQuery
    {
        public string Sheet;
        public string Selection;
        public string Orientation;

        public override string ToString()
        {
            return string.Format("{0}!{1}", this.Sheet, this.Selection);
        }
    }
}
