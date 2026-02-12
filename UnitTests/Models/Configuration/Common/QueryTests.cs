using RedditEmblemAPI.Models.Configuration.Common;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.BatchGetRequest;

namespace UnitTests.Models.Configuration.Common
{
    public class QueryTests
    {
        #region ToString

        [Test]
        public void Query_ToString()
        {
            IQuery query = new Query()
            {
                Sheet = "Sheet",
                Selection = "Selection",
                Orientation = MajorDimensionEnum.ROWS
            };

            string expected = "Sheet!Selection";
            Assert.That(query.ToString(), Is.EqualTo(expected));
        }

        #endregion ToString
    }
}
