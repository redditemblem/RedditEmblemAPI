using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_StatValueCSVsTests
    {
        private const string STATS_FIELD_NAME = "stats";
        private const string VALUES_FIELD_NAME = "values";

        #region StatValueCSVs_Int_Any

        [TestMethod]
        public void StatValueCSVs_Int_Any_Null()
        {
            IEnumerable<string> data = new List<string>();
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty, string.Empty };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING, UnitTestConsts.WHITESPACE_STRING };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_StatsOnly()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", string.Empty };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_ValuesOnly()
        {
            IEnumerable<string> data = new List<string>() { string.Empty, "1" };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_MismatchedStatsCount()
        {
            IEnumerable<string> data = new List<string>() { "Stat1,Stat2", "1" };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_MismatchedValuesCount()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "1,2" };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<ParameterLengthsMismatchedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_Alphanumeric()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "test" };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_0()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "0" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_0_IncludeZeroValues()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "0" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME, true);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat1"));
            Assert.AreEqual<int>(0, output["Stat1"]);
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_1()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "1" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat1"));
            Assert.AreEqual<int>(1, output["Stat1"]);
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "-1" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat1"));
            Assert.AreEqual<int>(-1, output["Stat1"]);
        }

        [TestMethod]
        public void StatValueCSVs_Int_Any_1_5()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "1.5" };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        #endregion StatValueCSVs_Int_Any
    }
}
