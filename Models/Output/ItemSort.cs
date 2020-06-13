namespace RedditEmblemAPI.Models.Output
{
    public class ItemSort
    {
        public string DisplayName { get; set; }

        public string SortAttribute { get; set; }

        public bool IsDeepSort { get; set; }

        public ItemSort(string name, string sortAttribute, bool isDeepSort)
        {
            this.DisplayName = name;
            this.SortAttribute = sortAttribute;
            this.IsDeepSort = isDeepSort;
        }
    }
}
