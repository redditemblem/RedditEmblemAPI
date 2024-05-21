using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output;
using RedditEmblemAPI.Services.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class DataParser_NamedStatDictionaryTests
    {
        #region Int_Any

        [TestMethod]
        public void NamedStatDictionary_Int_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_Any_EmptyString()
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

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_Any_Whitespace()
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

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_Any_Alphanumerical()
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

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.NamedStatDictionary_Int_Any(configs, data));
        }

        [TestMethod]
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
            Assert.AreEqual<int>(0, output.Count);
        }

        [TestMethod]
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
            Assert.AreEqual<int>(0, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Int_Any_1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);
            Assert.AreEqual<int>(1, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Int_Any_Neg1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "-1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_Any(configs, data);
            Assert.AreEqual<int>(-1, output["Stat 1"]);
        }

        [TestMethod]
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
            Assert.AreEqual(2, output.Count);
            Assert.AreEqual<int>(1, output["Stat 1"]);
            Assert.AreEqual<int>(2, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<int>(1, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual<int>(2, output.Count);
            Assert.AreEqual<int>(0, output["Stat 1"]);
            Assert.AreEqual<int>(1, output["Stat 2"]);
        }

        #endregion Int_Any

        #region OptionalInt_Any

        [TestMethod]
        public void NamedStatDictionary_OptionalInt_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyIntegerException>(() => DataParser.NamedStatDictionary_OptionalInt_Any(configs, data));
        }

        [TestMethod]
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
            Assert.AreEqual<int>(0, output.Count);
        }

        [TestMethod]
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

            Assert.AreEqual<int>(0, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_OptionalInt_Any_1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.AreEqual<int>(1, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_OptionalInt_Any_Neg1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "-1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_OptionalInt_Any(configs, data);

            Assert.AreEqual<int>(-1, output["Stat 1"]);
        }

        [TestMethod]
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
            Assert.AreEqual(2, output.Count);
            Assert.AreEqual<int>(1, output["Stat 1"]);
            Assert.AreEqual<int>(2, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<int>(1, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual<int>(2, output.Count);
            Assert.AreEqual<int>(0, output["Stat 1"]);
            Assert.AreEqual<int>(1, output["Stat 2"]);
        }

        #endregion OptionalInt_Any

        #region Int_NonZeroPositive

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_EmptyString()
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_Whitespace()
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_Alphanumerical()
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_0()
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1" };

            IDictionary<string, int> output = DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data);
            Assert.AreEqual<int>(1, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Int_NonZeroPositive_Neg1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "-1" };

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        [TestMethod]
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
            Assert.AreEqual(2, output.Count);
            Assert.AreEqual<int>(1, output["Stat 1"]);
            Assert.AreEqual<int>(2, output["Stat 2"]);
        }

        [TestMethod]
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

            Assert.ThrowsException<NonZeroPositiveIntegerException>(() => DataParser.NamedStatDictionary_Int_NonZeroPositive(configs, data));
        }

        #endregion Int_NonZeroPositive

        #region NamedStatDictionary_Decimal_Any

        [TestMethod]
        public void NamedStatDictionary_Decimal_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatDictionary_Decimal_Any(configs, data, true));
        }

        [TestMethod]
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
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<decimal>(0, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Decimal_Any_1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<decimal>(1, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Decimal_Any_1_5()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1.5" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<decimal>(1.5m, output["Stat 1"]);
        }

        [TestMethod]
        public void NamedStatDictionary_Decimal_Any_Neg1()
        {
            IEnumerable<NamedStatConfig> configs = new List<NamedStatConfig>()
            {
                new NamedStatConfig()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "-1" };

            IDictionary<string, decimal> output = DataParser.NamedStatDictionary_Decimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual<decimal>(-1, output["Stat 1"]);
        }

        [TestMethod]
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
            Assert.AreEqual(2, output.Count);
            Assert.AreEqual<decimal>(1, output["Stat 1"]);
            Assert.AreEqual<decimal>(2, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual(1, output.Count);
            Assert.IsFalse(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(1, output["Stat 2"]);
        }

        [TestMethod]
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
            Assert.AreEqual(2, output.Count);
            Assert.AreEqual<decimal>(0, output["Stat 1"]);
            Assert.AreEqual<decimal>(1, output["Stat 2"]);
        }

        #endregion NamedStatDictionary_Decimal_Any

        #region NamedStatValueDictionary_OptionalDecimal_Any

        [TestMethod]
        public void NamedStatValueDictionary_OptionalDecimal_Any_NoConfigs()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>();
            IEnumerable<string> data = new List<string>();

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(0, output["Stat 1"].Value);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(0, output["Stat 1"].Value);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(0, output["Stat 1"].Value);
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data));
        }

        [TestMethod]
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

            Assert.ThrowsException<AnyDecimalException>(() => DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true));
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(0, output.Count);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(0, output["Stat 1"].Value);
        }

        [TestMethod]
        public void NamedStatValueDictionary_OptionalDecimal_Any_1()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1" };

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(1, output["Stat 1"].Value);
        }

        [TestMethod]
        public void NamedStatValueDictionary_OptionalDecimal_Any_1_5()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "1.5" };

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(1.5m, output["Stat 1"].Value);
        }

        [TestMethod]
        public void NamedStatValueDictionary_OptionalDecimal_Any_Neg1()
        {
            IEnumerable<NamedStatConfig_Displayed> configs = new List<NamedStatConfig_Displayed>()
            {
                new NamedStatConfig_Displayed()
                {
                    SourceName = "Stat 1",
                    Value = 0
                }
            };
            IEnumerable<string> data = new List<string>() { "-1" };

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(-1, output["Stat 1"].Value);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(2, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(1, output["Stat 1"].Value);
            Assert.IsTrue(output.ContainsKey("Stat 2"));
            Assert.AreEqual<decimal>(2, output["Stat 2"].Value);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data);
            Assert.AreEqual(1, output.Count);
            Assert.IsFalse(output.ContainsKey("Stat 1"));
            Assert.IsTrue(output.ContainsKey("Stat 2"));
            Assert.AreEqual<decimal>(1, output["Stat 2"].Value);
        }

        [TestMethod]
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

            IDictionary<string, NamedStatValue> output = DataParser.NamedStatValueDictionary_OptionalDecimal_Any(configs, data, true);
            Assert.AreEqual(2, output.Count);
            Assert.IsTrue(output.ContainsKey("Stat 1"));
            Assert.AreEqual<decimal>(0, output["Stat 1"].Value);
            Assert.IsTrue(output.ContainsKey("Stat 2"));
            Assert.AreEqual<decimal>(1, output["Stat 2"].Value);
        }

        #endregion NamedStatValueDictionary_OptionalDecimal_Any
    }
}