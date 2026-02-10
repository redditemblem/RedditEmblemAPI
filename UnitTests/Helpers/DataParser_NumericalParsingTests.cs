using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    public class DataParser_NumericalParsingTests
    {
        #region Int_Any

        [Test]
        public void Int_Any_WithInput_IndexOutOfRange()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_WithInput_Null"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        public void Int_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, index, "Int_Any_InvalidInputs"));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void Int_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Any(data, index, "Int_Any_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Any

        #region OptionalInt_Any

        [Test]
        public void OptionalInt_Any_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_IndexOutOfBounds_NoDefault");

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Any_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_WithInput_IndexOutOfBounds_WithDefault", 1);

            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalInt_Any_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_EmptyStringInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_Any_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void OptionalInt_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_Any

        #region Int_Positive

        [Test]
        public void Int_Positive_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_Null"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        [TestCase("-1")]
        public void Int_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_InvalidInputs"));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void Int_Positive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Positive(data, index, "Int_Positive_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Positive

        #region OptionalInt_Positive

        [Test]
        public void OptionalInt_Positive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Null_NoDefault");
            
            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_Null_WithDefault", 1);
            
            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalInt_Positive_ValidInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_ValidInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_Positive_ValidInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_ValidInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void OptionalInt_Positive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("-1")]
        public void OptionalInt_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_InvalidInputs"));
        }

        #endregion OptionalInt_Positive

        #region Int_NonZeroPositive

        [Test]
        public void Int_NonZeroPositive_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_Null"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        [TestCase("0")]
        [TestCase("-1")]
        public void Int_NonZeroPositive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_InvalidInputs"));
        }

        [TestCase("1", 1)]
        public void Int_NonZeroPositive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_NonZeroPositive

        #region OptionalInt_NonZeroPositive

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Null_NoDefault");
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Null_WithDefault", 2);
            
            Assert.That(value, Is.EqualTo(2));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_EmptyString_NoDefault");
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_EmptyString_WithDefault", 2);
            
            Assert.That(value, Is.EqualTo(2));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Whitespace_NoDefault");
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Whitespace_WithDefault", 2);
            
            Assert.That(value, Is.EqualTo(2));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_0"));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_1");
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion OptionalInt_NonZeroPositive

        #region Int_Negative

        [Test]
        public void Int_Negative_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Null"));
        }

        [Test]
        public void Int_Negative_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_EmptyString"));
        }

        [Test]
        public void Int_Negative_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Whitespace"));
        }

        [Test]
        public void Int_Negative_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Alphabetical"));
        }

        [Test]
        public void Int_Negative_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "1.5" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Decimal"));
        }

        [Test]
        public void Int_Negative_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_0"));
        }

        [Test]
        public void Int_Negative_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_1"));
        }

        [Test]
        public void Int_Negative_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.Int_Negative(data, index, "Int_Negative_WithInput_Neg1");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        #endregion Int_Negative

        #region OptionalInt_Negative

        [Test]
        public void OptionalInt_Negative_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Null_NoDefault");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Null_WithDefault", -2);
            
            Assert.That(value, Is.EqualTo(-2));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_EmptyString_NoDefault");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_EmptyString_WithDefault", -2);
            
            Assert.That(value, Is.EqualTo(-2));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Whitespace_NoDefault");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Whitespace_WithDefault", -2);
            
            Assert.That(value, Is.EqualTo(-2));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_0"));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_1"));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_Neg1");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        #endregion OptionalInt_Negative

        #region Decimal_Any

        [Test]
        public void Decimal_Any_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Null"));
        }

        [Test]
        public void Decimal_Any_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_EmptyString"));
        }

        [Test]
        public void Decimal_Any_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Whitespace"));
        }

        [Test]
        public void Decimal_Any_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Alphabetical"));
        }

        [Test]
        public void Decimal_Any_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "1.5" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Decimal");
            
            Assert.That(value, Is.EqualTo(1.5m));
        }

        [Test]
        public void Decimal_Any_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_0");
            
            Assert.That(value, Is.EqualTo(0.0m));
        }

        [Test]
        public void Decimal_Any_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_1");
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void Decimal_Any_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_Neg1");
            
            Assert.That(value, Is.EqualTo(-1.0m));
        }

        #endregion Int_Any

        #region OptionalDecimal_Any

        [Test]
        public void OptionalDecimal_Any_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Null_NoDefault");
            
            Assert.That(value, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Null_WithDefault", 1.0m);
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_EmptyString_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Whitespace_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Any_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion OptionalDecimal_Any

        #region Decimal_Positive

        [Test]
        public void Decimal_Positive_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Null"));
        }

        [Test]
        public void Decimal_Positive_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_EmptyString"));
        }

        [Test]
        public void Decimal_Positive_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Whitespace"));
        }

        [Test]
        public void Decimal_Positive_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Alphabetical"));
        }

        [Test]
        public void Decimal_Positive_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "1.5" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Decimal");
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [Test]
        public void Decimal_Positive_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void Decimal_Positive_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void Decimal_Positive_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Neg1"));
        }

        #endregion Decimal_Positive

        #region OptionalDecimal_Positive

        [Test]
        public void OptionalDecimal_Positive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Null_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_EmptyString_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Whitespace_WithDefault", 1.0m);
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_0");
            Assert.AreEqual<decimal>(0.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Positive_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<PositiveDecimalException>(() => DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_Positive

        #region Decimal_NonZeroPositive

        [Test]
        public void Decimal_NonZeroPositive_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Null"));
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_EmptyString"));
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Whitespace"));
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Alphabetical"));
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "0.5" };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Decimal");
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_0"));
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void Decimal_NonZeroPositive_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion Decimal_NonZeroPositive

        #region OptionalDecimal_NonZeroPositive

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Null_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_EmptyString_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Whitespace_WithDefault", 0.5m);
            Assert.AreEqual<decimal>(0.5m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_0"));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_NonZeroPositive

        #region Decimal_OneOrGreater

        [Test]
        public void Decimal_OneOrGreater_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Null"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_EmptyString"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Whitespace"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Alphabetical"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "0.5" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Decimal"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_0"));
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void Decimal_OneOrGreater_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_WithInput_Neg1"));
        }

        #endregion Decimal_OneOrGreater

        #region OptionalDecimal_OneOrGreater

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Null_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_EmptyString_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Whitespace_WithDefault", 1.5m);
            Assert.AreEqual<decimal>(1.5m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_0"));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_1");
            Assert.AreEqual<decimal>(1.0m, value);
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            Assert.ThrowsException<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_WithInput_Neg1"));
        }

        #endregion OptionalDecimal_OneOrGreater

        #region Decimal_Negative

        [Test]
        public void Decimal_Negative_WithInput_Null()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Null"));
        }

        [Test]
        public void Decimal_Negative_WithInput_EmptyString()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_EmptyString"));
        }

        [Test]
        public void Decimal_Negative_WithInput_Whitespace()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Whitespace"));
        }

        [Test]
        public void Decimal_Negative_WithInput_Alphabetical()
        {
            IEnumerable<string> data = new List<string>() { "test" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Alphabetical"));
        }

        [Test]
        public void Decimal_Negative_WithInput_Decimal()
        {
            IEnumerable<string> data = new List<string>() { "0.5" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_OneOrGreater_WithInput_Decimal"));
        }

        [Test]
        public void Decimal_Negative_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_0"));
        }

        [Test]
        public void Decimal_Negative_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_1"));
        }

        [Test]
        public void Decimal_Negative_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.Decimal_Negative(data, index, "Decimal_Negative_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion Decimal_Negative

        #region OptionalDecimal_Negative

        [Test]
        public void OptionalDecimal_Negative_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Null_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_Null_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Null_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_EmptyString_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_EmptyString_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_EmptyString_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { string.Empty };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_EmptyString_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_Whitespace_NoDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Whitespace_NoDefault");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_Whitespace_WithDefault()
        {
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Whitespace_WithDefault", -0.5m);
            Assert.AreEqual<decimal>(-0.5m, value);
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_0()
        {
            IEnumerable<string> data = new List<string>() { "0" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_0"));
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_1()
        {
            IEnumerable<string> data = new List<string>() { "1" };
            int index = 0;

            Assert.ThrowsException<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_1"));
        }

        [Test]
        public void OptionalDecimal_Negative_WithInput_Neg1()
        {
            IEnumerable<string> data = new List<string>() { "-1" };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_WithInput_Neg1");
            Assert.AreEqual<decimal>(-1.0m, value);
        }

        #endregion OptionalDecimal_Negative
    }
}