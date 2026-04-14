using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Models.Configuration.Common
{
    public class IntTupleConverterTests
    {
        #region Setup

        private JsonSerializer Serializer;

        [OneTimeSetUp]
        public void SetUp()
        {
            Serializer = new JsonSerializer();
        }

        #endregion Setup

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("a")]
        [TestCase("test")]
        [TestCase("0, 0")]
        [TestCase("(0, 0)")]
        public void IntTupleConverter_WithInput_SingleString(string input)
        {
            string json = $@"{{ ""Value"": ""{input}"" }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }

        [TestCase(-2)]
        [TestCase(-1)]
        public void IntTupleConverter_WithInput_NegativeSingleInteger(int input)
        {
            string json = $@"{{ ""Value"": {input} }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void IntTupleConverter_WithInput_PositiveSingleInteger(int input)
        {
            string json = $@"{{ ""Value"": { input } }}";
            TextReader reader = new StringReader(json);

            IntTupleConverterResult? result = (IntTupleConverterResult?)Serializer.Deserialize(reader, typeof(IntTupleConverterResult));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo((0, input)));
        }

        [TestCase("0", "n")]
        [TestCase("n", "0")]
        [TestCase("n", "n")]
        public void IntTupleConverter_WithInput_StringArray(string input1, string input2)
        {
            string json = $@"{{ ""Value"": [""{input1}"", ""{input2}""] }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }

        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, -1)]
        public void IntTupleConverter_WithInput_NegativeIntegerArray(int input1, int input2)
        {
            string json = $@"{{ ""Value"": [{input1}, {input2}] }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void IntTupleConverter_WithInput_PositiveIntegerArray(int input1, int input2)
        {
            string json = $@"{{ ""Value"": [{ input1 }, { input2 }] }}";
            TextReader reader = new StringReader(json);

            IntTupleConverterResult? result = (IntTupleConverterResult?)Serializer.Deserialize(reader, typeof(IntTupleConverterResult));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo((input1, input2)));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void IntTupleConverter_WithInput_TooShortIntegerArray(int input)
        {
            string json = $@"{{ ""Value"": [{ input }] }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 1)]
        [TestCase(2, 2, 2)]
        public void IntTupleConverter_WithInput_TooLongIntegerArray(int input1, int input2, int input3)
        {
            string json = $@"{{ ""Value"": [{input1}, {input2}, {input3}] }}";
            TextReader reader = new StringReader(json);

            Assert.Throws<InvalidIntTupleConverterInputException>(() => Serializer.Deserialize(reader, typeof(IntTupleConverterResult)));
        }
    }

    internal class IntTupleConverterResult
    {
        [JsonConverter(typeof(IntTupleConverter))]
        public (int, int) Value { get; set; }
    }
}
