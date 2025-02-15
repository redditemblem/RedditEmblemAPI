using Newtonsoft.Json;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.System.Tags;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.System.Interfaces;

namespace UnitTests.Models.System.Interfaces
{
    [TestClass]
    public class IMatchableTests
    {
        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_EmptyDictionary()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(0, dict.Count);

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.AreEqual("{}", serialized);
        }

        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_SingleItemDictionary_Unmatched()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" }
                        }
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(1, dict.Count);

            Tag tag = dict["Tag 1"];
            Assert.IsFalse(tag.Matched);

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.AreEqual("{}", serialized);
        }

        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_SingleItemDictionary_Matched()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" }
                        }
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(1, dict.Count);

            Tag tag = dict["Tag 1"];
            tag.FlagAsMatched();
            Assert.IsTrue(tag.Matched);

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.IsTrue(serialized.Contains("\"Name\":\"Tag 1\""));
        }

        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_MultipleItemDictionary_AllUnmatched()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" },
                            new List<object>(){ "Tag 2" },
                            new List<object>(){ "Tag 3" },
                        }
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(3, dict.Count);

            Assert.IsFalse(dict.Values.Any(t => t.Matched));

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.AreEqual("{}", serialized);
        }

        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_MultipleItemDictionary_AllMatched()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" },
                            new List<object>(){ "Tag 2" },
                            new List<object>(){ "Tag 3" },
                        }
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(3, dict.Count);

            foreach (var tag in dict.Values)
                tag.FlagAsMatched();
            Assert.IsTrue(dict.Values.All(t => t.Matched));

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.IsTrue(serialized.Contains("\"Name\":\"Tag 1\""));
            Assert.IsTrue(serialized.Contains("\"Name\":\"Tag 2\""));
            Assert.IsTrue(serialized.Contains("\"Name\":\"Tag 3\""));
        }

        [TestMethod]
        public void OmitUnmatchedObjectsFromIMatchableDictionaryConverter_MultipleItemDictionary_MixedMatched()
        {
            TagsConfig config = new TagsConfig()
            {
                Queries = new List<Query>()
                {
                    new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tag 1" },
                            new List<object>(){ "Tag 2" },
                            new List<object>(){ "Tag 3" },
                        }
                    }
                },
                Name = 0
            };

            IReadOnlyDictionary<string, Tag> dict = Tag.BuildDictionary(config);
            Assert.AreEqual(3, dict.Count);

            dict["Tag 1"].FlagAsMatched();
            Assert.IsTrue(dict.Values.Where(t => t.Matched).Count() == 1);

            string serialized = JsonConvert.SerializeObject(dict, new OmitUnmatchedObjectsFromIMatchableDictionaryConverter());
            Assert.IsTrue(serialized.Contains("\"Name\":\"Tag 1\""));
            Assert.IsFalse(serialized.Contains("\"Name\":\"Tag 2\""));
            Assert.IsFalse(serialized.Contains("\"Name\":\"Tag 3\""));
        }
    }
}
