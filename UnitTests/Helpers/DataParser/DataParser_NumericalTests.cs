using RedditEmblemAPI.Helpers;
using RedditEmblemAPI.Models.Exceptions.Validation;

namespace UnitTests.Helpers
{
    public class DataParser_NumericalTests
    {
        #region Int_Any

        [Test]
        public void Int_Any_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, indices, nameof(Int_Any_MultiSet_WithInput_Null)));
        }

        [Test]
        public void Int_Any_MultiSet_WithInput_IndexOutOfRange()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, indices, nameof(Int_Any_MultiSet_WithInput_IndexOutOfRange)));
        }

        [Test]
        public void Int_Any_MultiSet_WithInput_SetOutOfRange()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>{ "1" }
            };
            (int, int) indices = (1, 0);

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, indices, nameof(Int_Any_MultiSet_WithInput_SetOutOfRange)));
        }

        [TestCase(0, 0, -1)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 2, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(2, 0, 4)]
        public void Int_Any_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>{ "-1", "0", "1" },
                new List<string>{ "2", "3" },
                new List<string>{ "4" },
            };
            (int, int) indices = (setIndex, cellIndex);

            int actual = DataParser.Int_Any(data, indices, nameof(Int_Any_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Int_Any_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, index, nameof(Int_Any_WithInput_Null)));
        }

        [Test]
        public void Int_Any_WithInput_IndexOutOfRange()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, index, nameof(Int_Any_WithInput_IndexOutOfRange)));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("1.5")]
        public void Int_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.Int_Any(data, index, nameof(Int_Any_InvalidInputs)));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void Int_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Any(data, index, nameof(Int_Any_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Any

        #region OptionalInt_Any

        [Test]
        public void OptionalInt_Any_MultiSet_WithInput_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Any(data, indices, nameof(OptionalInt_Any_MultiSet_WithInput_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Any_MultiSet_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Any(data, indices, nameof(OptionalInt_Any_MultiSet_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Any_MultiSet_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Any(data, indices, nameof(OptionalInt_Any_MultiSet_WithInput_IndexOutOfBounds_WithDefault), 1);

            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase(0, 0, -1)]
        [TestCase(0, 1, 0)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(2, 0, 3)]
        public void OptionalInt_Any_MultiSet(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>() { "-1", "0"},
                new List<string>() { "1", "2" },
                new List<string>() { "3" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int value = DataParser.OptionalInt_Any(data, indices, nameof(OptionalInt_Any_MultiSet));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalInt_Any_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_WithInput_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Any_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Any_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_WithInput_IndexOutOfBounds_WithDefault), 1);

            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalInt_Any_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_EmptyStringInputs_NoDefault));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_Any_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_EmptyStringInputs_WithDefault), expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        [TestCase("1.5")]
        public void OptionalInt_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyIntegerException>(() => DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_InvalidInputs)));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void OptionalInt_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Any(data, index, nameof(OptionalInt_Any_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_Any

        #region Int_Positive

        [Test]
        public void Int_Positive_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, indices, nameof(Int_Positive_MultiSet_WithInput_Null)));
        }

        [Test]
        public void Int_Positive_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, indices, nameof(Int_Positive_MultiSet_WithInput_IndexOutOfBounds)));
        }

        [Test]
        public void Int_Positive_MultiSet_WithInput_SetOutOfRange()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "1" }
            };
            (int, int) indices = (1, 0);

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, indices, nameof(Int_Positive_MultiSet_WithInput_SetOutOfRange)));
        }

        [TestCase(0, 1, 0)]
        [TestCase(0, 2, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(2, 0, 4)]
        public void Int_Positive_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1", "0", "1" },
                new List<string>(){ "2", "3" },
                new List<string>(){ "4" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int actual = DataParser.Int_Positive(data, indices, nameof(Int_Positive_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Int_Positive_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, nameof(Int_Positive_WithInput_Null)));
        }

        [Test]
        public void Int_Positive_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, nameof(Int_Positive_WithInput_IndexOutOfBounds)));
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

            Assert.Throws<PositiveIntegerException>(() => DataParser.Int_Positive(data, index, nameof(Int_Positive_InvalidInputs)));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void Int_Positive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Positive(data, index, nameof(Int_Positive_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Positive

        #region OptionalInt_Positive

        [Test]
        public void OptionalInt_Positive_MultiSet_WithInput_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Positive(data, indices, nameof(OptionalInt_Positive_MultiSet_WithInput_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_MultiSet_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Positive(data, indices, nameof(OptionalInt_Positive_MultiSet_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_MultiSet_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Positive(data, indices, nameof(OptionalInt_Positive_MultiSet_WithInput_IndexOutOfBounds_WithDefault), 1);

            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase(0, 1, 0)]
        [TestCase(0, 2, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(2, 0, 4)]
        public void OptionalInt_Positive_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1", "0", "1" },
                new List<string>(){ "2", "3" },
                new List<string>(){ "4" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int value = DataParser.OptionalInt_Positive(data, indices, nameof(OptionalInt_Positive_MultiSet_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalInt_Positive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_WithInput_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_WithInput_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalInt_Positive_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_WithInput_IndexOutOfBounds_WithDefault), 1);
            
            Assert.That(value, Is.EqualTo(1));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalInt_Positive_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_EmptyStringInputs_NoDefault));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_Positive_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_EmptyStringInputs_WithDefault), expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("-1")]
        [TestCase("1.5")]
        public void OptionalInt_Positive_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<PositiveIntegerException>(() => DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_InvalidInputs)));
        }

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        public void OptionalInt_Positive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Positive(data, index, nameof(OptionalInt_Positive_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_Positive

        #region Int_NonZeroPositive

        [Test]
        public void Int_NonZeroPositive_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, indices, nameof(Int_NonZeroPositive_MultiSet_WithInput_Null)));
        }

        [Test]
        public void Int_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, indices, nameof(Int_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds)));
        }

        [TestCase(0, 0, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(2, 0, 4)]
        public void Int_NonZeroPositive_MultiSet_ValidInput(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>{ "1" },
                new List<string>{ "2", "3" },
                new List<string>{ "4" },
            };
            (int, int) indices = (setIndex, cellIndex);

            int actual = DataParser.Int_NonZeroPositive(data, indices, nameof(Int_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Int_NonZeroPositive_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, nameof(Int_NonZeroPositive_WithInput_Null)));
        }

        [Test]
        public void Int_NonZeroPositive_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, nameof(Int_NonZeroPositive_WithInput_IndexOutOfBounds)));
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

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.Int_NonZeroPositive(data, index, nameof(Int_NonZeroPositive_InvalidInputs)));
        }

        [TestCase("1", 1)]
        public void Int_NonZeroPositive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_NonZeroPositive(data, index, nameof(Int_NonZeroPositive_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_NonZeroPositive

        #region OptionalInt_NonZeroPositive

        [Test]
        public void OptionalInt_NonZeroPositive_MultiSet_WithInput_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_NonZeroPositive(data, indices, nameof(OptionalInt_NonZeroPositive_MultiSet_WithInput_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_NonZeroPositive(data, indices, nameof(OptionalInt_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_NonZeroPositive(data, indices, nameof(OptionalInt_NonZeroPositive_MultiSet_WithInput_IndexOutOfBounds_WithDefault), 2);

            Assert.That(value, Is.EqualTo(2));
        }

        [TestCase(0, 2, 1)]
        [TestCase(1, 0, 2)]
        [TestCase(1, 1, 3)]
        [TestCase(2, 0, 4)]
        public void OptionalInt_NonZeroPositive_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1", "0", "1" },
                new List<string>(){ "2", "3" },
                new List<string>(){ "4" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int value = DataParser.OptionalInt_NonZeroPositive(data, indices, nameof(OptionalInt_NonZeroPositive_MultiSet_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_WithInput_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.EqualTo(1));
        }

        [Test]
        public void OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_WithInput_IndexOutOfBounds_WithDefault), 2);
            
            Assert.That(value, Is.EqualTo(2));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalInt_NonZeroPositive_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_EmptyStringInputs_NoDefault));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 2)]
        [TestCase("   ", 2)]
        public void OptionalInt_NonZeroPositive_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_EmptyStringInputs_WithDefault), expectedDefault);
            
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

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_InvalidInputs)));
        }

        [TestCase("1", 1)]
        public void OptionalInt_NonZeroPositive_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_NonZeroPositive(data, index, nameof(OptionalInt_NonZeroPositive_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_NonZeroPositive

        #region Int_Negative

        [Test]
        public void Int_Negative_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, indices, nameof(Int_Negative_MultiSet_WithInput_Null)));
        }


        [Test]
        public void Int_Negative_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, indices, nameof(Int_Negative_MultiSet_WithInput_IndexOutOfBounds)));
        }

        [TestCase(1, 0, -1)]
        public void Int_Negative_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "2", "3" },
                new List<string>(){ "-1", "0", "1" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int actual = DataParser.Int_Negative(data, indices, nameof(Int_Negative_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Int_Negative_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, nameof(Int_Negative_WithInput_Null)));
        }

        [Test]
        public void Int_Negative_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, nameof(Int_Negative_WithInput_IndexOutOfBounds)));
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

            Assert.Throws<NegativeIntegerException>(() => DataParser.Int_Negative(data, index, nameof(Int_Negative_InvalidInputs)));
        }


        [TestCase("-1", -1)]
        public void Int_Negative_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.Int_Negative(data, index, nameof(Int_Negative_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Negative

        #region OptionalInt_Negative

        [Test]
        public void OptionalInt_Negative_MultiSet_WithInput_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Negative(data, indices, nameof(OptionalInt_Negative_MultiSet_WithInput_Null_NoDefault));

            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_MultiSet_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Negative(data, indices, nameof(OptionalInt_Negative_MultiSet_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_MultiSet_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            int value = DataParser.OptionalInt_Negative(data, indices, nameof(OptionalInt_Negative_MultiSet_WithInput_IndexOutOfBounds_NoDefault), -2);

            Assert.That(value, Is.EqualTo(-2));
        }

        [TestCase(1, 0, -2)]
        [TestCase(1, 1, -1)]
        public void OptionalInt_Negative_MultiSet_ValidInputs(int setIndex, int cellIndex, int expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "0", "1" },
                new List<string>(){ "-2", "-1" }
            };
            (int, int) indices = (setIndex, cellIndex);

            int value = DataParser.OptionalInt_Negative(data, indices, nameof(OptionalInt_Negative_MultiSet_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_WithInput_Null_NoDefault));

            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_WithInput_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.EqualTo(-1));
        }

        [Test]
        public void OptionalInt_Negative_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_WithInput_IndexOutOfBounds_WithDefault), -2);
            
            Assert.That(value, Is.EqualTo(-2));
        }

        [TestCase("", -1)]
        [TestCase("   ", -1)]
        public void OptionalInt_Negative_EmptyStringInputs_NoDefault(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_EmptyStringInputs_NoDefault));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", -2)]
        [TestCase("   ", -2)]
        public void OptionalInt_Negative_EmptyStringInputs_WithDefault(string input, int expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_EmptyStringInputs_WithDefault), expectedDefault);
            
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

            Assert.Throws<NegativeIntegerException>(() => DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_InvalidInputs)));
        }

        [TestCase("-1", -1)]
        public void OptionalInt_Negative_ValidInputs(string input, int expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            int value = DataParser.OptionalInt_Negative(data, index, nameof(OptionalInt_Negative_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalInt_Negative

        #region Decimal_Any

        [Test]
        public void Decimal_Any_MultiSet_WithInput_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, indices, nameof(Decimal_Any_MultiSet_WithInput_Null)));
        }

        [Test]
        public void Decimal_Any_MultiSet_WithInput_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, indices, nameof(Decimal_Any_MultiSet_WithInput_IndexOutOfBounds)));
        }

        [TestCase(0, 0, -1.5)]
        [TestCase(0, 1, -1)]
        [TestCase(0, 2, -0.5)]
        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void Decimal_Any_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.Decimal_Any(data, indices, nameof(Decimal_Any_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Decimal_Any_WithInput_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, nameof(Decimal_Any_WithInput_Null)));
        }

        [Test]
        public void Decimal_Any_WithInput_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, nameof(Decimal_Any_WithInput_IndexOutOfBounds)));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        public void Decimal_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.Decimal_Any(data, index, nameof(Decimal_Any_InvalidInputs)));
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

            decimal value = DataParser.Decimal_Any(data, index, nameof(Decimal_Any_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Int_Any

        #region OptionalDecimal_Any

        [Test]
        public void OptionalDecimal_Any_MultiSet_WithInput_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            decimal actual = DataParser.OptionalDecimal_Any(data, indices, nameof(OptionalDecimal_Any_MultiSet_WithInput_Null_NoDefault));

            Assert.That(actual, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_MultiSet_WithInput_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal actual = DataParser.OptionalDecimal_Any(data, indices, nameof(OptionalDecimal_Any_MultiSet_WithInput_IndexOutOfBounds_NoDefault));

            Assert.That(actual, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_MultiSet_WithInput_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal actual = DataParser.OptionalDecimal_Any(data, indices, nameof(OptionalDecimal_Any_MultiSet_WithInput_IndexOutOfBounds_WithDefault), 1.0m);

            Assert.That(actual, Is.EqualTo(1.0m));
        }

        [TestCase(0, 0, -1.5)]
        [TestCase(0, 1, -1)]
        [TestCase(0, 2, -0.5)]
        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void OptionalDecimal_Any_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.OptionalDecimal_Any(data, indices, nameof(OptionalDecimal_Any_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalDecimal_Any_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_Null_NoDefault));

            Assert.That(value, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.EqualTo(0.0m));
        }

        [Test]
        public void OptionalDecimal_Any_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_IndexOutOfBounds_WithDefault), 1.0m);
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalDecimal_Any_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_EmptyStringInputs_NoDefault));

            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_Any_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_EmptyStringInputs_WithDefault), expectedDefault);
            
            Assert.That(value, Is.EqualTo(expectedDefault));
        }

        [TestCase("test")]
        public void OptionalDecimal_Any_InvalidInputs(string input)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            Assert.Throws<AnyDecimalException>(() => DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_InvalidInputs)));
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

            decimal value = DataParser.OptionalDecimal_Any(data, index, nameof(OptionalDecimal_Any_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Any

        #region Decimal_Positive

        [Test]
        public void Decimal_Positive_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, indices, nameof(Decimal_Positive_MultiSet_Null)));
        }

        [Test]
        public void Decimal_Positive_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, indices, nameof(Decimal_Positive_MultiSet_IndexOutOfBounds)));
        }

        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void Decimal_Positive_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.Decimal_Positive(data, indices, nameof(Decimal_Positive_MultiSet_IndexOutOfBounds));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Decimal_Positive_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, nameof(Decimal_Positive_Null)));
        }

        [Test]
        public void Decimal_Positive_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, nameof(Decimal_Positive_IndexOutOfBounds)));
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

            Assert.Throws<PositiveDecimalException>(() => DataParser.Decimal_Positive(data, index, nameof(Decimal_Positive_InvalidInputs)));
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

            decimal value = DataParser.Decimal_Positive(data, index, nameof(Decimal_Positive_ValidInputs));
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_Positive

        #region OptionalDecimal_Positive

        [Test]
        public void OptionalDecimal_Positive_MultiSet_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Positive(data, indices, nameof(OptionalDecimal_Positive_MultiSet_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalDecimal_Positive_MultiSet_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Positive(data, indices, nameof(OptionalDecimal_Positive_MultiSet_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalDecimal_Positive_MultiSet_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Positive(data, indices, nameof(OptionalDecimal_Positive_MultiSet_IndexOutOfBounds_WithDefault), 1.0m);

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void OptionalDecimal_Positive_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal value = DataParser.OptionalDecimal_Positive(data, indices, nameof(OptionalDecimal_Positive_MultiSet_IndexOutOfBounds_WithDefault));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalDecimal_Positive_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_Null_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalDecimal_Positive_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.Zero);
        }

        [Test]
        public void OptionalDecimal_Positive_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_IndexOutOfBounds_WithDefault), 1.0m);
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [TestCase("", 0)]
        [TestCase("   ", 0)]
        public void OptionalDecimal_Positive_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_EmptyStringInputs_NoDefault));
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_Positive_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_EmptyStringInputs_WithDefault), expectedDefault);

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

            Assert.Throws<PositiveDecimalException>(() => DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_InvalidInputs)));
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

            decimal value = DataParser.OptionalDecimal_Positive(data, index, nameof(OptionalDecimal_Positive_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Positive

        #region Decimal_NonZeroPositive

        [Test]
        public void Decimal_NonZeroPositive_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, indices, nameof(Decimal_NonZeroPositive_MultiSet_Null)));
        }

        [Test]
        public void Decimal_NonZeroPositive_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, indices, nameof(Decimal_NonZeroPositive_MultiSet_IndexOutOfBounds)));
        }

        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void Decimal_NonZeroPositive_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.Decimal_NonZeroPositive(data, indices, nameof(Decimal_NonZeroPositive_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Decimal_NonZeroPositive_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, nameof(Decimal_NonZeroPositive_Null)));
        }

        [Test]
        public void Decimal_NonZeroPositive_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, nameof(Decimal_NonZeroPositive_IndexOutOfBounds)));
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

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, nameof(Decimal_NonZeroPositive_InvalidInputs)));
        }

        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void Decimal_NonZeroPositive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_NonZeroPositive(data, index, nameof(Decimal_NonZeroPositive_ValidInputs));
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_NonZeroPositive

        #region OptionalDecimal_NonZeroPositive

        [Test]
        public void OptionalDecimal_NonZeroPositive_MultiSet_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, indices, nameof(OptionalDecimal_NonZeroPositive_MultiSet_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_MultiSet_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, indices, nameof(OptionalDecimal_NonZeroPositive_MultiSet_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_MultiSet_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, indices, nameof(OptionalDecimal_NonZeroPositive_MultiSet_IndexOutOfBounds_WithDefault), 2.0m);

            Assert.That(value, Is.EqualTo(2.0m));
        }

        [TestCase(2, 0, 0.5)]
        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void OptionalDecimal_NonZeroPositive_MultiSet_ValidValues(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, indices, nameof(OptionalDecimal_NonZeroPositive_MultiSet_ValidValues));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_NonZeroPositive_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_IndexOutOfBounds_WithDefault), 0.5m);
            
            Assert.That(value, Is.EqualTo(0.5m));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_NonZeroPositive_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_EmptyStringInputs_NoDefault));
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 0.5)]
        [TestCase("   ", 0.5)]
        public void OptionalDecimal_NonZeroPositive_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_EmptyStringInputs_WithDefault), expectedDefault);
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

            Assert.Throws<NonZeroPositiveDecimalException>(() => DataParser.Decimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_InvalidInputs)));
        }

        [TestCase("0.5", 0.5)]
        [TestCase("1", 1)]
        [TestCase("1.", 1)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_NonZeroPositive_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_NonZeroPositive(data, index, nameof(OptionalDecimal_NonZeroPositive_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_NonZeroPositive

        #region Decimal_OneOrGreater

        [Test]
        public void Decimal_OneOrGreater_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, indices, nameof(Decimal_OneOrGreater_MultiSet_Null)));
        }

        [Test]
        public void Decimal_OneOrGreater_MultiSet_IndexOutOfBound()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, indices, nameof(Decimal_OneOrGreater_MultiSet_IndexOutOfBound)));
        }

        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void Decimal_OneOrGreater_MultiSet_ValidInputs(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.Decimal_OneOrGreater(data, indices, nameof(Decimal_OneOrGreater_MultiSet_ValidInputs));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Decimal_OneOrGreater_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, nameof(Decimal_OneOrGreater_Null)));
        }

        [Test]
        public void Decimal_OneOrGreater_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, nameof(Decimal_OneOrGreater_IndexOutOfBounds)));
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

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.Decimal_OneOrGreater(data, index, nameof(Decimal_OneOrGreater_InvalidInputs)));
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        public void Decimal_OneOrGreater_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_OneOrGreater(data, index, nameof(Decimal_OneOrGreater_ValidInputs));
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_OneOrGreater

        #region OptionalDecimal_OneOrGreater

        [Test]
        public void OptionalDecimal_OneOrGreater_MultiSet_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, indices, nameof(OptionalDecimal_OneOrGreater_MultiSet_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_MultiSet_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, indices, nameof(OptionalDecimal_OneOrGreater_MultiSet_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_MultiSet_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, indices, nameof(OptionalDecimal_OneOrGreater_MultiSet_IndexOutOfBounds_WithDefault), 1.5m);

            Assert.That(value, Is.EqualTo(1.5m));
        }

        [TestCase(2, 1, 1)]
        [TestCase(2, 2, 1.5)]
        public void OptionalDecimal_OneOrGreater_MultiSet_ValidValues(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, indices, nameof(OptionalDecimal_OneOrGreater_MultiSet_ValidValues));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_Null_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(1.0m));
        }

        [Test]
        public void OptionalDecimal_OneOrGreater_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_IndexOutOfBounds_WithDefault), 1.5m);

            Assert.That(value, Is.EqualTo(1.5m));
        }

        [TestCase("", 1)]
        [TestCase("   ", 1)]
        public void OptionalDecimal_OneOrGreater_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_EmptyStringInputs_NoDefault));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", 1.5)]
        [TestCase("   ", 1.5)]
        public void OptionalDecimal_OneOrGreater_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_EmptyStringInputs_WithDefault), expectedDefault);
            
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

            Assert.Throws<OneOrGreaterDecimalException>(() => DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_InvalidInputs)));
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        public void OptionalDecimal_OneOrGreater_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_OneOrGreater(data, index, nameof(OptionalDecimal_OneOrGreater_ValidInputs));
            
            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_OneOrGreater

        #region Decimal_Negative

        [Test]
        public void Decimal_Negative_MultiSet_Null()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, indices, nameof(Decimal_Negative_MultiSet_Null)));
        }

        [Test]
        public void Decimal_Negative_MultiSet_IndexOutOfBounds()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, indices, nameof(Decimal_Negative_MultiSet_IndexOutOfBounds)));
        }

        [TestCase(0, 0, -1.5)]
        [TestCase(0, 1, -1)]
        [TestCase(0, 2, -0.5)]
        public void Decimal_Negative_MultiSet_ValidValues(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal actual = DataParser.Decimal_Negative(data, indices, nameof(Decimal_Negative_MultiSet_ValidValues));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Decimal_Negative_Null()
        {
            IEnumerable<string> data = null;
            int index = 0;

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, nameof(Decimal_Negative_Null)));
        }

        [Test]
        public void Decimal_Negative_IndexOutOfBounds()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, nameof(Decimal_Negative_IndexOutOfBounds)));
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

            Assert.Throws<NegativeDecimalException>(() => DataParser.Decimal_Negative(data, index, nameof(Decimal_Negative_InvalidInputs)));
        }

        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("-1.5", -1.5)]
        public void Decimal_Negative_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.Decimal_Negative(data, index, nameof(Decimal_Negative_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion Decimal_Negative

        #region OptionalDecimal_Negative

        [Test]
        public void OptionalDecimal_Negative_MultiSet_Null_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = null;
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Negative(data, indices, nameof(OptionalDecimal_Negative_MultiSet_Null_NoDefault));

            Assert.That(value, Is.EqualTo(-1.0m));
        }

        [Test]
        public void OptionalDecimal_Negative_MultiSet_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Negative(data, indices, nameof(OptionalDecimal_Negative_MultiSet_IndexOutOfBounds_NoDefault));

            Assert.That(value, Is.EqualTo(-1.0m));
        }

        [Test]
        public void OptionalDecimal_Negative_MultiSet_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>();
            (int, int) indices = (0, 0);

            decimal value = DataParser.OptionalDecimal_Negative(data, indices, nameof(OptionalDecimal_Negative_MultiSet_IndexOutOfBounds_NoDefault), -0.5m);

            Assert.That(value, Is.EqualTo(-0.5m));
        }

        [TestCase(0, 0, -1.5)]
        [TestCase(0, 1, -1)]
        [TestCase(0, 2, -0.5)]
        public void OptionalDecimal_Negative_MultiSet_ValidValues(int setIndex, int cellIndex, decimal expected)
        {
            IEnumerable<IEnumerable<string>> data = new List<List<string>>()
            {
                new List<string>(){ "-1.5", "-1", "-0.5" },
                new List<string>(){ "0" },
                new List<string>(){ "0.5", "1", "1.5" }
            };
            (int, int) indices = (setIndex, cellIndex);

            decimal value = DataParser.OptionalDecimal_Negative(data, indices, nameof(OptionalDecimal_Negative_MultiSet_ValidValues));

            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void OptionalDecimal_Negative_Null_NoDefault()
        {
            IEnumerable<string> data = null;
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_Null_NoDefault));

            Assert.That(value, Is.EqualTo(-1.0m));
        }

        [Test]
        public void OptionalDecimal_Negative_IndexOutOfBounds_NoDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_IndexOutOfBounds_NoDefault));
            
            Assert.That(value, Is.EqualTo(-1.0m));
        }

        [Test]
        public void OptionalDecimal_Negative_IndexOutOfBounds_WithDefault()
        {
            IEnumerable<string> data = new List<string>();
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_IndexOutOfBounds_WithDefault), -0.5m);

            Assert.That(value, Is.EqualTo(-0.5m));
        }

        [TestCase("", -1.0)]
        [TestCase("   ", -1.0)]
        public void OptionalDecimal_Negative_EmptyStringInputs_NoDefault(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_EmptyStringInputs_NoDefault));

            Assert.That(value, Is.EqualTo(expected));
        }

        [TestCase("", -0.5)]
        [TestCase("   ", -0.5)]
        public void OptionalDecimal_Negative_EmptyStringInputs_WithDefault(string input, decimal expectedDefault)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_EmptyStringInputs_WithDefault), expectedDefault);
            
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

            Assert.Throws<NegativeDecimalException>(() => DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_InvalidInputs)));
        }

        [TestCase("-0.5", -0.5)]
        [TestCase("-1", -1)]
        [TestCase("-1.5", -1.5)]
        public void OptionalDecimal_Negative_ValidInputs(string input, decimal expected)
        {
            IEnumerable<string> data = new List<string>() { input };
            int index = 0;

            decimal value = DataParser.OptionalDecimal_Negative(data, index, nameof(OptionalDecimal_Negative_ValidInputs));

            Assert.That(value, Is.EqualTo(expected));
        }

        #endregion OptionalDecimal_Negative
    }
}