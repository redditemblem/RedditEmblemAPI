using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Emblems;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.System
{
    public class EngageAttackTests
    {
        #region Constants

        private const string INPUT_NAME = "Engage Attack Test";

        #endregion Constants

        [Test]
        public void Constructor_RequiredFields_WithInputNull()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>() { };

            Assert.Throws<RequiredValueNotProvidedException>(() => new EngageAttack(config, data));
        }

        [Test]
        public void Constructor_RequiredFields()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IEngageAttack attack = new EngageAttack(config, data);

            Assert.That(attack.Name, Is.EqualTo(INPUT_NAME));
        }

        #region OptionalField_SpriteURL

        [Test]
        public void Constructor_OptionalField_SpriteURL_EmptyString()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            IEngageAttack attack = new EngageAttack(config, data);

            Assert.That(attack.SpriteURL, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_SpriteURL()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                SpriteURL = 1
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                UnitTestConsts.IMAGE_URL
            };

            EngageAttack attack = new EngageAttack(config, data);

            Assert.That(attack.SpriteURL, Is.EqualTo(UnitTestConsts.IMAGE_URL));
        }

        #endregion OptionalField_SpriteURL

        #region OptionalField_TextFields

        [Test]
        public void Constructor_OptionalField_TextFields_EmptyString()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty,
                string.Empty
            };

            IEngageAttack attack = new EngageAttack(config, data);

            Assert.That(attack.TextFields, Is.Empty);
        }

        [Test]
        public void Constructor_OptionalField_TextFields()
        {
            string textField1 = "Text Field 1";
            string textField2 = "Text Field 2";

            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0,
                TextFields = new List<int>() { 1, 2 }
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                textField1,
                textField2
            };

            IEngageAttack attack = new EngageAttack(config, data);

            IEnumerable<string> expected = new List<string>() { textField1, textField2 };
            Assert.That(attack.TextFields, Is.EqualTo(expected));
        }

        #endregion OptionalField_TextFields

        #region FlagAsMatched

        [Test]
        public void FlagAsMatched()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Name = 0
            };

            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME
            };

            IEngageAttack attack = new EngageAttack(config, data);

            Assert.That(attack.Matched, Is.False);

            attack.FlagAsMatched();

            Assert.That(attack.Matched, Is.True);
        }

        #endregion FlagAsMatched

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(null);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = null,
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
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
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_DuplicateName()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME },
                            new List<object>(){ INPUT_NAME }
                        }
                    }
                },
                Name = 0
            };

            Assert.Throws<EngageAttackProcessingException>(() => EngageAttack.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary_WithInput_Invalid()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME, "NotURL" }
                        }
                    }
                },
                Name = 0,
                SpriteURL = 1
            };

            Assert.Throws<EngageAttackProcessingException>(() => EngageAttack.BuildDictionary(config));
        }

        [Test]
        public void BuildDictionary()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_NAME }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultiQuery()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 1" },
                            new List<object>(){ "Engage Attack 2" }
                        }
                    },
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 3" },
                            new List<object>(){ "Engage Attack 4" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary

        #region MatchNames

        [Test]
        public void MatchNames_UnmatchedName()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 1" },
                            new List<object>(){ "Engage Attack 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 3" };

            Assert.Throws<UnmatchedEngageAttackException>(() => EngageAttack.MatchNames(dict, names));
        }

        [Test]
        public void MatchNames_SingleMatch()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 1" },
                            new List<object>(){ "Engage Attack 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1" };

            List<IEngageAttack> matches = EngageAttack.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(1));
            Assert.That(matches.First().Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 1" },
                            new List<object>(){ "Engage Attack 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1", "Engage Attack 2" };

            List<IEngageAttack> matches = EngageAttack.MatchNames(dict, names);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.True);
            Assert.That(matches[1].Matched, Is.True);
        }

        [Test]
        public void MatchNames_MultipleMatches_DoNotSetMatchedStatus()
        {
            EngageAttacksConfig config = new EngageAttacksConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Engage Attack 1" },
                            new List<object>(){ "Engage Attack 2" }
                        }
                    }
                },
                Name = 0
            };

            IDictionary<string, IEngageAttack> dict = EngageAttack.BuildDictionary(config);
            IEnumerable<string> names = new List<string>() { "Engage Attack 1", "Engage Attack 2" };

            List<IEngageAttack> matches = EngageAttack.MatchNames(dict, names, false);

            Assert.That(matches.Count, Is.EqualTo(2));
            Assert.That(matches[0].Matched, Is.False);
            Assert.That(matches[1].Matched, Is.False);
        }

        #endregion MatchNames
    }
}