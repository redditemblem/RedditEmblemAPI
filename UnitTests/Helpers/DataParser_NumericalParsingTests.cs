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

        [TestCase("test")]
        [TestCase("1.5")]
        public void OptionalInt_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.OptionalInt_Any(data, index, "OptionalInt_Any_InvalidInputs"));
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
        public void Int_Positive_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, "Int_Positive_WithInput_IndexOutOfBounds"));
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
        public void OptionalInt_Positive_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_WithInput_IndexOutOfBounds_WithDefault", 1);
            
            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalInt_Positive_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_EmptyStringInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_Positive_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("-1")]
        [TestCase("1.5")]
        public void OptionalInt_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.OptionalInt_Positive(data, index, "OptionalInt_Positive_InvalidInputs"));
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

        #endregion OptionalInt_Positive

        #region Int_NonZeroPositive

        [Test]
        public void Int_NonZeroPositive_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, "Int_NonZeroPositive_WithInput_IndexOutOfBounds"));
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
        public void OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_WithDefault", 2);
            
            Assert.That(value, Is.EqualTo(2));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_NonZeroPositive_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_EmptyStringInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 2)]
        [TestCase("   ", 2)]
        public void OptionalInt_NonZeroPositive_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("-1")]
        [TestCase("0")]
        [TestCase("1.5")]
        public void OptionalInt_NonZeroPositive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_InvalidInputs"));
        }

        [TestCase("1", 1)]
        public void OptionalInt_NonZeroPositive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, "OptionalInt_NonZeroPositive_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_NonZeroPositive

        #region Int_Negative

        [Test]
        public void Int_Negative_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_WithInput_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        [TestCase("1")]
        [TestCase("0")]
        public void Int_Negative_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, "Int_Negative_InvalidInputs"));
        }


        [TestCase("-1", -1)]
        public void Int_Negative_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Negative(data, index, "Int_Negative_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Negative

        #region OptionalInt_Negative

        [Test]
        public void OptionalInt_Negative_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_WithInput_IndexOutOfBounds_WithDefault", -2);
            
            Assert.That(value, Is.EqualTo(-2));
        }

        [TestCase("", -1)]
        [TestCase("   ", -1)]
        public void OptionalInt_Negative_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_EmptyStringInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", -2)]
        [TestCase("   ", -2)]
        public void OptionalInt_Negative_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("1.5")]
        public void OptionalInt_Negative_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_InvalidInputs"));
        }

        [TestCase("-1", -1)]
        public void OptionalInt_Negative_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, "OptionalInt_Negative_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_Negative

        #region Decimal_Any

        [Test]
        public void Decimal_Any_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_WithInput_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        public void Decimal_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, "Decimal_Any_InvalidInputs"));
        }

        [TestCase("-1.5", -1.5)]
        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("0.5", 0.5)]
        [TestCase("1.5", 1.5)]
        public void Decimal_Any_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_Any(data, index, "Decimal_Any_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Any

        #region OptionalDecimal_Any

        [Test]
        public void OptionalDecimal_Any_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_IndexOutOfBounds_WithDefault", 1.0m);
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalDecimal_Any_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_EmptyStringInputs_NoDefault");

            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_Any_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        public void OptionalDecimal_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_InvalidInputs"));
        }

        [TestCase("-1.5", -1.5)]
        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("0.5", 0.5)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_Any_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, "OptionalDecimal_Any_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Any

        #region Decimal_Positive

        [Test]
        public void Decimal_Positive_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-1")]
        [TestCase("-0.5")]
        public void Decimal_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, "Decimal_Positive_InvalidInputs"));
        }

        [TestCase("0", 0)]
        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void Decimal_Positive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_Positive(data, index, "Decimal_Positive_WithInput_Decimal");
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_Positive

        #region OptionalDecimal_Positive

        [Test]
        public void OptionalDecimal_Positive_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_IndexOutOfBounds_NoDefault");

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalDecimal_Positive_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_IndexOutOfBounds_WithDefault", 1.0m);
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalDecimal_Positive_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_EmptyStringInputs_NoDefault");
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_Positive_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_EmptyStringInputs_WithDefault", expectedDefault);

            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-1")]
        [TestCase("-0.5")]
        public void OptionalDecimal_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveDecimalException>(() => DataParser.OptionalDecimal_Positive(data, index, "Decimal_Positive_InvalidInputs"));
        }

        [TestCase("0", 0)]
        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_Positive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, "OptionalDecimal_Positive_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Positive

        #region Decimal_NonZeroPositive

        [Test]
        public void Decimal_NonZeroPositive_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-0.5")]
        [TestCase("0")]
        public void Decimal_NonZeroPositive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_InvalidInputs"));
        }

        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void Decimal_NonZeroPositive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, "Decimal_NonZeroPositive_ValidInputs");
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_NonZeroPositive

        #region OptionalDecimal_NonZeroPositive

        [Test]
        public void OptionalDecimal_NonZeroPositive_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_IndexOutOfBounds_WithDefault", 0.5m);
            
            Assert.That(value, Is.EqualTo(0.5m));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_NonZeroPositive_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_EmptyStringInputs_NoDefault");
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 0.5)]
        [TestCase("   ", 0.5)]
        public void OptionalDecimal_NonZeroPositive_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_WithInput_EmptyString_WithDefault", expectedDefault);
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-1")]
        [TestCase("-0.5")]
        [TestCase("0")]
        public void OptionalDecimal_NonZeroPositive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_InvalidInputs"));
        }

        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_NonZeroPositive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, "OptionalDecimal_NonZeroPositive_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_NonZeroPositive

        #region Decimal_OneOrGreater

        [Test]
        public void Decimal_OneOrGreater_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-1")]
        [TestCase("-0.5")]
        [TestCase("0")]
        [TestCase("0.5")]
        public void Decimal_OneOrGreater_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_InvalidInputs"));
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        public void Decimal_OneOrGreater_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_OneOrGreater(data, index, "Decimal_OneOrGreater_ValidInputs");
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_OneOrGreater

        #region OptionalDecimal_OneOrGreater

        [Test]
        public void OptionalDecimal_OneOrGreater_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_IndexOutOfBounds_NoDefault");

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_IndexOutOfBounds_WithDefault", 1.5m);

            Assert.That(value, Is.EqualTo(1.5m));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_OneOrGreater_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_EmptyStringInputs_NoDefault");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1.5)]
        [TestCase("   ", 1.5)]
        public void OptionalDecimal_OneOrGreater_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("-1.5")]
        [TestCase("-1")]
        [TestCase("-0.5")]
        [TestCase("0")]
        [TestCase("0.5")]
        public void OptionalDecimal_OneOrGreater_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_InvalidInputs"));
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_OneOrGreater_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, "OptionalDecimal_OneOrGreater_ValidInputs");
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_OneOrGreater

        #region Decimal_Negative

        [Test]
        public void Decimal_Negative_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_IndexOutOfBounds"));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        [TestCase("1")]
        [TestCase("0.5")]
        [TestCase("0")]
        public void Decimal_Negative_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, "Decimal_Negative_InvalidInputs"));
        }

        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("-1.5", -1.5)]
        public void Decimal_Negative_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_Negative(data, index, "Decimal_Negative_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_Negative

        #region OptionalDecimal_Negative

        [Test]
        public void OptionalDecimal_Negative_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_IndexOutOfBounds_NoDefault");
            
            Assert.That(value, Is.EqualTo(-1.0m));
        }

        [Test]
        public void OptionalDecimal_Negative_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_IndexOutOfBounds_WithDefault", -0.5m);

            Assert.That(value, Is.EqualTo(-0.5m));
        }

        [TestCase("", -1.0)]
        [TestCase("   ", -1.0)]
        public void OptionalDecimal_Negative_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_EmptyStringInputs_NoDefault");

            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", -0.5)]
        [TestCase("   ", -0.5)]
        public void OptionalDecimal_Negative_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_EmptyStringInputs_WithDefault", expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("1.5")]
        [TestCase("1")]
        [TestCase("0.5")]
        [TestCase("0")]
        public void OptionalDecimal_Negative_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_InvalidInputs"));
        }

        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("-1.5", -1.5)]
        public void OptionalDecimal_Negative_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, "OptionalDecimal_Negative_ValidInputs");

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Negative
    }
}