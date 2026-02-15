using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    public class DataParser_NamedStatDictionaryTests
    {
        #region Int_Any

        [Test]
        public void NamedStatDictionary_Int_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);
            
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_Int_Any_Null()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            Assert.Throws<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        public void NamedStatDictionary_Int_Any_InvalidInputs(string input)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            Assert.Throws<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_Int_Any_0()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);
            
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_Int_Any_0_KeepZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data, true);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 1"], Is.EqualTo(0));
        }

        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void NamedStatDictionary_Int_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 1"], Is.EqualTo(expected));
        }

        [Test]
        public void NamedStatDictionary_Int_Any_MultipleConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "1", "2" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(1));
            Assert.That(output["Stat 2"], Is.EqualTo(2));
        }

        [Test]
        public void NamedStatDictionary_Int_Any_MultipleConfigs_WithZero()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 2"], Is.EqualTo(1));
        }

        [Test]
        public void NamedStatDictionary_Int_Any_MultipleConfigs_WithZero_KeepZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data, true);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(0));
            Assert.That(output["Stat 2"], Is.EqualTo(1));
        }

        #endregion Int_Any

        #region OptionalInt_Any

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_Null()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_EmptyString()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { string.Empty };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_Whitespace()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_Alphanumerical()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "test" };

            Assert.Throws<AnyIntegerException>(() => DataParser.NamedStatDictionary_OptionalInt_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_0()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_0_KeepZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data, true);

            Assert.That(output["Stat 1"], Is.EqualTo(0));
        }

        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        public void NamedStatDictionary_OptionalInt_Any_ValidInputs(string input, int expected)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.That(output["Stat 1"], Is.EqualTo(expected));
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_MultipleConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "1", "2" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(1));
            Assert.That(output["Stat 2"], Is.EqualTo(2));
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_MultipleConfigs_WithZero()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 2"], Is.EqualTo(1));
        }

        [Test]
        public void NamedStatDictionary_OptionalInt_Any_MultipleConfigs_WithZero_KeepZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data, true);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(0));
            Assert.That(output["Stat 2"], Is.EqualTo(1));
        }

        #endregion OptionalInt_Any

        #region Int_NonZeroPositive

        [Test]
        public void NamedStatDictionary_Int_NonZeroPositive_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_Int_NonZeroPositive_Null()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("0")]
        [TestCase("-1")]
        public void NamedStatDictionary_Int_NonZeroPositive_InvalidInputs(string input)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestCase("1", 1)]
        public void NamedStatDictionary_Int_NonZeroPositive_ValidInputs(string input, int expected)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 1"], Is.EqualTo(expected));
        }

        [Test]
        public void NamedStatDictionary_Int_NonZeroPositive_MultipleConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "1", "2" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(1));
            Assert.That(output["Stat 2"], Is.EqualTo(2));
        }

        [Test]
        public void NamedStatDictionary_Int_NonZeroPositive_MultipleConfigs_WithZero()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            Assert.Throws<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        #endregion Int_NonZeroPositive

        #region NamedStatDictionary_Decimal_Any

        [Test]
        public void NamedStatDictionary_Decimal_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Null()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Null_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_EmptyString()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { string.Empty };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_EmptyString_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { string.Empty };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Whitespace()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Whitespace_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Alphanumerical()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "test" };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_Alphanumerical_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "test" };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_0()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_0_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data, true);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 1"], Is.Zero);
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        [TestCase("-1", -1)]
        public void NamedStatDictionary_Decimal_Any_ValidInputs(string input, decimal expected)
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output["Stat 1"], Is.EqualTo(expected));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_MultipleConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "1", "2" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.EqualTo(1m));
            Assert.That(output["Stat 2"], Is.EqualTo(2m));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_MultipleConfigs_WithZero()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);

            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.False);
            Assert.That(output["Stat 2"], Is.EqualTo(1m));
        }

        [Test]
        public void NamedStatDictionary_Decimal_Any_MultipleConfigs_WithZero_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data, true);

            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output["Stat 1"], Is.Zero);
            Assert.That(output["Stat 2"], Is.EqualTo(1m));
        }

        #endregion NamedStatDictionary_Decimal_Any

        #region NamedStatValueDictionary_OptionalDecimal_Any

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Null()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Null_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>();

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.Zero);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_EmptyString()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { string.Empty };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_EmptyString_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { string.Empty };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.Zero);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Whitespace()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Whitespace_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { UnitTestConsts.WHITESPACE_STRING };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.Zero);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Alphanumerical()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "test" };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data));
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Alphanumerical_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "test" };

            Assert.Throws<AnyDecimalException>(() => DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true));
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_0()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_0_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "0" };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.Zero);
        }

        [TestCase("1", 1)]
        [TestCase("1.5", 1.5)]
        [TestCase("-1", -1)]
        [TestCase("-1.5", -1.5)]
        public void NamedStatValueDictionary_OptionalDecimal_ValidInputs(string input, decimal expected)
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { input };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.EqualTo(expected));
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_MultipleConfigs()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "1", "2" };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            
            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 2"), Is.True);
            Assert.That(output["Stat 2"].Value, Is.EqualTo(2));
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_MultipleConfigs_WithZero()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            
            Assert.That(output.Count, Is.EqualTo(1));
            Assert.That(output.ContainsKey("Stat 1"), Is.False);
            Assert.That(output.ContainsKey("Stat 2"), Is.True);
            Assert.That(output["Stat 2"].Value, Is.EqualTo(1));
        }

        [Test]
        public void NamedStatValueDictionary_OptionalDecimal_Any_MultipleConfigs_WithZero_IncludeZeroValues()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                },
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 2",
                    Value = 1
                }
            };
            IEnumerable<string> data = new List<string>() { "0", "1" };

            IDictionary<string, INamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            
            Assert.That(output.Count, Is.EqualTo(2));
            Assert.That(output.ContainsKey("Stat 1"), Is.True);
            Assert.That(output["Stat 1"].Value, Is.Zero);
            Assert.That(output.ContainsKey("Stat 2"), Is.True);
            Assert.That(output["Stat 2"].Value, Is.EqualTo(1));
        }

        #endregion NamedStatValueDictionary_OptionalDecimal_Any
    }
}