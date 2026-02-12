using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.TileObjects;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class TileObjectTests
    {
        #region Constants

        private const string INPUT_NAME = "Tile Object Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_IndexOutOfBounds()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new TileObject(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Name, Is.EqualTo(INPUT_NAME));
            Assert.That(tileObj.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #region OptionalField_Size

        [Test]
        public void Constructor_OptionalField_Size_EmptyString()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Size = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Size, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_OptionalField_Size_0()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Size = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "0"
            };

            Assert.Throws<NonZeroPositiveIntegerException>(() => new TileObject(config, data));
        }

        [Test]
        public void Constructor_OptionalField_Size()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Size = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "2"
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Size, Is.EqualTo(2));
        }

        #endregion OptionalField_Size

        #region OptionalField_Layer

        [Test]
        public void Constructor_OptionalField_Layer_EmptyString()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Layer = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Layer, Is.EqualTo(TileObjectLayer.Below));
        }

        [Test]
        public void Constructor_OptionalField_Layer_UnmatchedLayerType()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Layer = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "FakeLayerType"
            };

            Assert.Throws<UnmatchedTileObjectLayerException>(() => new TileObject(config, data));
        }

        [TestCase("Below", TileObjectLayer.Below)]
        [TestCase("Above", TileObjectLayer.Above)]
        public void Constructor_OptionalField_Layer_ValidInputs(string input, TileObjectLayer expected)
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Layer = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                input
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Layer, Is.EqualTo(expected));
        }

        #endregion OptionalField_Layer

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                TextFields = new List<int>() { 2, 3 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                textField1,
                textField2
            };

            ITileObject tileObj = new TileObject(config, data);

            List<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(tileObj.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region OptionalField_Range

        [Test]
        public void Constructor_OptionalField_Range_NullConfig()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            ITileObject tileObj = new TileObject(config, data);
            Assert.That(tileObj.Range, Is.Not.Null);

            ITileObjectRange range = tileObj.Range;
            Assert.That(range.Minimum, Is.Zero);
            Assert.That(range.Maximum, Is.Zero);
        }

        [Test]
        public void Constructor_OptionalField_Range()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                Range = new TileObjectRangeConfig()
                {
                    Minimum = 2,
                    Maximum = 3
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "1",
                "2"
            };

            ITileObject tileObj = new TileObject(config, data);
            Assert.That(tileObj.Range, Is.Not.Null);

            ITileObjectRange range = tileObj.Range;
            Assert.That(range.Minimum, Is.EqualTo(1));
            Assert.That(range.Maximum, Is.EqualTo(2));
        }

        #endregion OptionalField_Range

        #region OptionalField_HPModifier

        [Test]
        public void Constructor_OptionalField_HPModifier_EmptyString()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                HPModifier = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.HPModifier, Is.Zero);
        }

        [TestCase("-10", -10)]
        [TestCase("10", 10)]
        public void Constructor_OptionalField_HPModifier(string input, int expected)
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                HPModifier = 2
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                input
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.HPModifier, Is.EqualTo(expected));
        }

        #endregion OptionalField_HPModifier

        #region OptionalField_CombatStatModifiers

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 2 },
                    new NamedStatConfig { SourceName = stat2, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.CombatStatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_CombatStatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                CombatStatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 2 },
                    new NamedStatConfig { SourceName = stat2, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "1",
                "-1"
            };

            ITileObject tileObj = new TileObject(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>()
            {
                { stat1, 1 },
                { stat2, -1 }
            };
            Assert.That(tileObj.CombatStatModifiers, Is.EqualTo(expected));
        }

        #endregion OptionalField_CombatStatModifiers

        #region OptionalField_StatModifiers

        [Test]
        public void Constructor_OptionalField_StatModifiers_EmptyString()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 2 },
                    new NamedStatConfig { SourceName = stat2, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                string.Empty,
                string.Empty
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.StatModifiers, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_StatModifiers()
        {
            string stat1 = "Stat 1";
            string stat2 = "Stat 2";

            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1,
                StatModifiers = new List<NamedStatConfig>()
                {
                    new NamedStatConfig { SourceName = stat1, Value = 2 },
                    new NamedStatConfig { SourceName = stat2, Value = 3 }
                }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL,
                "1",
                "-1"
            };

            ITileObject tileObj = new TileObject(config, data);

            IDictionary<string, int> expected = new Dictionary<string, int>()
            {
                { stat1, 1 },
                { stat2, -1 }
            };
            Assert.That(tileObj.StatModifiers, Is.EqualTo(expected));
        }

        #endregion OptionalField_StatModifiers

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            ITileObject tileObj = new TileObject(config, data);

            Assert.That(tileObj.Matched, Is.False);

            tileObj.FlagAsMatched();

            Assert.That(tileObj.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, ITileObject> dict = TileObject.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = null,
                Name = 0,
                SpriteURL = 1
            };

            IDictionary<string, ITileObject> dict = TileObject.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            IDictionary<string, ITileObject> dict = TileObject.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, UnitTestConsts.IMAGE_URL },
                            new List<object>(){ INPUT_NAME, UnitTestConsts.IMAGE_URL }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.Throws<TileObjectProcessingException>(() => TileObject.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "NotAURL" }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.Throws<TileObjectProcessingException>(() => TileObject.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, UnitTestConsts.IMAGE_URL }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            IDictionary<string, ITileObject> dict = TileObject.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            TileObjectsConfig config = new TileObjectsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tile Object 1", UnitTestConsts.IMAGE_URL },
                            new List<object>(){ "Tile Object 2", UnitTestConsts.IMAGE_URL }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tile Object 3", UnitTestConsts.IMAGE_URL },
                            new List<object>(){ "Tile Object 4", UnitTestConsts.IMAGE_URL }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            IDictionary<string, ITileObject> dict = TileObject.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            string obj1Name = "Tile Object 1";
            string obj2Name = "Tile Object 2";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);

            IEnumerable<string> names = new List<string>() { obj2Name };
            ICoordinate coord = new Coordinate();

            Assert.Throws<UnmatchedTileObjectException>(() => TileObject.MatchNames(dict, names, coord));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            string obj1Name = "Tile Object 1";
            string obj2Name = "Tile Object 2";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            ITileObject obj2 = Substitute.For<ITileObject>();
            obj2.Name.Returns(obj2Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);
            dict.Add(obj2Name, obj2);

            IEnumerable<string> names = new List<string>() { obj1Name };
            ICoordinate coord = new Coordinate();
            List<ITileObject> matches = TileObject.MatchNames(dict, names, coord);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.Contains(obj1), Is.True);
            matches.First().Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            string obj1Name = "Tile Object 1";
            string obj2Name = "Tile Object 2";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            ITileObject obj2 = Substitute.For<ITileObject>();
            obj2.Name.Returns(obj2Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);
            dict.Add(obj2Name, obj2);

            IEnumerable<string> names = new List<string>() { obj1Name, obj2Name };
            ICoordinate coord = new Coordinate();
            List<ITileObject> matches = TileObject.MatchNames(dict, names, coord);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(obj1), Is.True);
            Assert.That(matches.Contains(obj2), Is.True);

            matches[0].Received(1).FlagAsMatched();
            matches[1].Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            string obj1Name = "Tile Object 1";
            string obj2Name = "Tile Object 2";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            ITileObject obj2 = Substitute.For<ITileObject>();
            obj2.Name.Returns(obj2Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);
            dict.Add(obj2Name, obj2);

            IEnumerable<string> names = new List<string>() { obj1Name, obj2Name };
            ICoordinate coord = new Coordinate();
            List<ITileObject> matches = TileObject.MatchNames(dict, names, coord, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches.Contains(obj1), Is.True);
            Assert.That(matches.Contains(obj2), Is.True);

            matches[0].DidNotReceive().FlagAsMatched();
            matches[1].DidNotReceive().FlagAsMatched();
        }

        #endregion MatchNames

        #region MatchName

        [Test]
        public void MatchName_UnmatchedName()
        {
            string obj1Name = "Tile Object 1";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);

            string name = "Tile Object 2";
            ICoordinate coord = new Coordinate();

            Assert.Throws<UnmatchedTileObjectException>(() => TileObject.MatchName(dict, name, coord));
        }

        [Test]
        public void MatchName()
        {
            string obj1Name = "Tile Object 1";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);

            ICoordinate coord = new Coordinate();
            ITileObject match = TileObject.MatchName(dict, obj1Name, coord);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(obj1));
            match.Received(1).FlagAsMatched();
        }

        [Test]
        public void MatchName_DoNotSetMatchedStatus()
        {
            string obj1Name = "Tile Object 1";

            ITileObject obj1 = Substitute.For<ITileObject>();
            obj1.Name.Returns(obj1Name);

            IDictionary<string, ITileObject> dict = new Dictionary<string, ITileObject>();
            dict.Add(obj1Name, obj1);

            ICoordinate coord = new Coordinate();
            ITileObject match = TileObject.MatchName(dict, obj1Name, coord, false);

            Assert.That(match, Is.Not.Null);
            Assert.That(match, Is.EqualTo(obj1));
            match.DidNotReceive().FlagAsMatched();
        }

        #endregion MatchName
    }
}
