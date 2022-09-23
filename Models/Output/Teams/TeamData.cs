namespace RedditEmblemAPI.Models.Output.Teams
{
    public class TeamData
    {
        /// <summary>
        /// The name of the team.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Flag to show the convoy button in the menu.
        /// </summary>
        public bool ShowConvoyLink { get; set; }

        /// <summary>
        /// Flag to show the shop button in the menu.
        /// </summary>
        public bool ShowShopLink { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="showConvoyLink"></param>
        /// <param name="showShopLink"></param>
        public TeamData(string teamName, bool showConvoyLink, bool showShopLink)
        {
            this.TeamName = teamName;
            this.ShowConvoyLink = showConvoyLink;
            this.ShowShopLink = showShopLink;
        }
    }
}
