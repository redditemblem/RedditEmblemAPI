using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Helpers
{
    public class DataParser_StatValueCSVsTests
    {
        private const string STATS_FIELD_NAME = "stats";
        private const string VALUES_FIELD_NAME = "values";

        #region StatValueCSVs_Int_Any

        [Test]
        public void StatValueCSVs_Int_Any_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("Stat1", "")]
        [TestCase("", "1")]
        public void StatValueCSVs_Int_Any_EmptyStringInputs(string input1, string input2)
        {
            IEnumerable<string> data = new List<string>() { input1, input2 };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.Throws<RequiredValueNotProvidedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestCase("Stat1,Stat2", "1")]
        [TestCase("Stat1", "1,2")]
        public void StatValueCSVs_Int_Any_MismatchedParameterLengths(string input1, string input2)
        {
            IEnumerable<string> data = new List<string>() { input1, input2 };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.Throws<ParameterLengthsMismatchedException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [TestCase("Stat1", "test")]
        [TestCase("Stat1", "1.5")]
        public void StatValueCSVs_Int_Any_InvalidInputs(string input1, string input2)
        {
            IEnumerable<string> data = new List<string>() { input1, input2 };
            int statsIndex = 0;
            int valuesIndex = 1;

            Assert.Throws<AnyIntegerException>(() => DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME));
        }

        [Test]
        public void StatValueCSVs_Int_Any_0()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "0" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);
            
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void StatValueCSVs_Int_Any_0_IncludeZeroValues()
        {
            IEnumerable<string> data = new List<string>() { "Stat1", "0" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME, true);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat1"), Is.True);
            Assert.That(output["Stat1"], Is.EqualTo(0));
        }

        [TestCase("Stat1", "1", 1)]
        [TestCase("Stat1", "-1", -1)]
        public void StatValueCSVs_Int_Any_ValidInputs(string input1, string input2, int expected)
        {
            IEnumerable<string> data = new List<string>() { input1, input2 };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey(input1), Is.True);
            Assert.That(output[input1], Is.EqualTo(expected));
        }

        [Test]
        public void StatValueCSVs_Int_Any_MultipleStatValues()
        {
            IEnumerable<string> data = new List<string>() { "Stat1,Stat2,Stat3", "3,2,1" };
            int statsIndex = 0;
            int valuesIndex = 1;

            IDictionary<string, int> output = DataParser.StatValueCSVs_Int_Any(data, statsIndex, STATS_FIELD_NAME, valuesIndex, VALUES_FIELD_NAME);

            Assert.That(output.Count, Is.EqualTo(3));
            Assert.That(output.ContainsKey("Stat1"), Is.True);
            Assert.That(output["Stat1"], Is.EqualTo(3));
            Assert.That(output.ContainsKey("Stat2"), Is.True);
            Assert.That(output["Stat2"], Is.EqualTo(2));
            Assert.That(output.ContainsKey("Stat3"), Is.True);
            Assert.That(output["Stat3"], Is.EqualTo(1));
        }

        #endregion StatValueCSVs_Int_Any
    }
}
