using RedditEmblemAPI.Models.Configuration.Common;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest;

namespace UnitTests.Models.Configuration.Common
{
    [TestClass]
    public class QueryTests
    {
        #region ToString

        [TestMethod]
        public void Query_ToString()
        {
            Query query = new Query()
            {
                Sheet = "Sheet",
                Selection = "Selection",
                Orientation = MajorDimensionEnum.ROWS
            };

            Assert.AreEqual<string>("Sheet!Selection", query.ToString());
        }

        #endregion ToString
    }
}
