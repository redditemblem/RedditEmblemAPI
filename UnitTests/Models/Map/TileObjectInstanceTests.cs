using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Configuration.Units;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Unmatched;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;
using RedditEmblemAPI.Models.Output.Units;

namespace UnitTests.Models.Map
{
    public class TileObjectInstanceTests
    {
        #region Constants

        private const string INPUT_NAME = "Tile Object 1";

        #endregion Constants

        #region SetUp

        private IDictionary<string, ITileObject> TILE_OBJECTS;
        private IMapObj MAP;

        [SetUp]
        public void SetUp()
        {
            this.TILE_OBJECTS = SetUp_Tile_Objects();
            this.MAP = SetUp_MapObj();
        }

        private IDictionary<string, ITileObject> SetUp_Tile_Objects()
        {
            string tileObj1Name = "Tile Object 1";
            ITileObject tileObj1 = Substitute.For<ITileObject>();
            tileObj1.Name.Returns(tileObj1Name);

            IDictionary<string, ITileObject> tileObjs = new Dictionary<string, ITileObject>();
            tileObjs.Add(tileObj1Name, tileObj1);

            return tileObjs;
        }

        private IMapObj SetUp_MapObj()
        {
            IMapObj map = Substitute.For<IMapObj>();
            MapConstantsConfig constants = new MapConstantsConfig() { CoordinateFormat = CoordinateFormat.XY };
            map.Constants.Returns(constants);

            ITile tile1 = Substitute.For<ITile>();
            tile1.Coordinate.Returns(new Coordinate(CoordinateFormat.XY, 1, 1));
            tile1.TileObjects.Returns(new List<ITileObjectInstance>());
            map.GetTileByCoord(1, 1).Returns(tile1);

            ITile tile2 = Substitute.For<ITile>();
            tile2.Coordinate.Returns(new Coordinate(CoordinateFormat.XY, 2, 1));
            tile2.TileObjects.Returns(new List<ITileObjectInstance>());
            map.GetTileByCoord(2, 1).Returns(tile2);

            ITile tile3 = Substitute.For<ITile>();
            tile3.Coordinate.Returns(new Coordinate(CoordinateFormat.XY, 1, 2));
            tile3.TileObjects.Returns(new List<ITileObjectInstance>());
            map.GetTileByCoord(1, 2).Returns(tile3);

            ITile tile4 = Substitute.For<ITile>();
            tile4.Coordinate.Returns(new Coordinate(CoordinateFormat.XY, 2, 2));
            tile4.TileObjects.Returns(new List<ITileObjectInstance>());
            map.GetTileByCoord(2, 2).Returns(tile4);

            return map;
        }

        #endregion SetUp

        [Test]
        public void Constructor_IndexOutOfBounds()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>();

            Assert.Throws<RequiredValueNotProvidedException>(() => new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_MissingCoordinate()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                string.Empty
            };

            Assert.Throws<RequiredValueNotProvidedException>(() => new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_InvalidCoordinate()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "FakeCoord"
            };

            Assert.Throws<XYCoordinateFormattingException>(() => new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_UnmatchedTileObjectName()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                "Tile Object 2",
                "1,1"
            };

            Assert.Throws<UnmatchedTileObjectException>(() => new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_Size1TileObject()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,1"
            };

            ITileObject tileObj = TILE_OBJECTS[INPUT_NAME];
            tileObj.Size.Returns(1);

            ITileObjectInstance instance = new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS);

            Assert.That(instance.ID, Is.EqualTo(tileObjectID));
            Assert.That(instance.TileObject, Is.EqualTo(tileObj));
            Assert.That(instance.HP, Is.Null);
            Assert.That(instance.AttackRange, Is.Empty);

            ITile tile = MAP.GetTileByCoord(1, 1);
            Assert.That(tile, Is.Not.Null);

            ITile[] origins = new ITile[1]{ tile };
            IEnumerable<ITileObjectInstance> expected = new List<ITileObjectInstance>() { instance };

            Assert.That(instance.OriginTiles, Is.EqualTo(origins));
            Assert.That(tile.TileObjects, Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_Size2TileObject()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,1"
            };

            ITileObject tileObj = TILE_OBJECTS[INPUT_NAME];
            tileObj.Size.Returns(2);

            ITileObjectInstance instance = new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS);

            Assert.That(instance.ID, Is.EqualTo(tileObjectID));
            Assert.That(instance.TileObject, Is.EqualTo(tileObj));
            Assert.That(instance.HP, Is.Null);
            Assert.That(instance.AttackRange, Is.Empty);

            ITile tile1 = MAP.GetTileByCoord(1, 1);
            ITile tile2 = MAP.GetTileByCoord(2, 1);
            ITile tile3 = MAP.GetTileByCoord(1, 2);
            ITile tile4 = MAP.GetTileByCoord(2, 2);

            Assert.That(tile1, Is.Not.Null);
            Assert.That(tile2, Is.Not.Null);
            Assert.That(tile3, Is.Not.Null);
            Assert.That(tile4, Is.Not.Null);

            ITile[] origins = new ITile[4] { tile1, tile2, tile3, tile4 };
            IEnumerable<ITileObjectInstance> expected = new List<ITileObjectInstance>() { instance };

            Assert.That(instance.OriginTiles, Is.EqualTo(origins));
            Assert.That(tile1.TileObjects, Is.EqualTo(expected));
            Assert.That(tile2.TileObjects, Is.EqualTo(expected));
            Assert.That(tile3.TileObjects, Is.EqualTo(expected));
            Assert.That(tile4.TileObjects, Is.EqualTo(expected));
        }

        #region OptionalFields_HP

        [Test]
        public void Constructor_OptionalFields_HP_EmptyStringCurrent()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1,
                HP = new HPConfig()
                {
                    Current = 2,
                    Maximum = 3,
                    RemainingBars = 4
                }
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,1",
                string.Empty
            };

            ITileObjectInstance instance = new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS);

            Assert.That(instance.ID, Is.EqualTo(tileObjectID));
            Assert.That(instance.HP, Is.Null);
        }

        [Test]
        public void Constructor_OptionalFields_HP()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Name = 0,
                Coordinate = 1,
                HP = new HPConfig()
                {
                    Current = 2,
                    Maximum = 3,
                    RemainingBars = 4
                }
            };

            int tileObjectID = 1;
            IEnumerable<string> data = new List<string>()
            {
                INPUT_NAME,
                "1,1",
                "10",
                "15",
                "1"
            };

            ITileObjectInstance instance = new TileObjectInstance(config, tileObjectID, data, MAP, TILE_OBJECTS);

            Assert.That(instance.ID, Is.EqualTo(tileObjectID));
            Assert.That(instance.HP, Is.Not.Null);

            IHealthPoints hp = instance.HP;

            Assert.That(hp.Current, Is.EqualTo(10));
            Assert.That(hp.Maximum, Is.EqualTo(15));
            Assert.That(hp.RemainingBars, Is.EqualTo(1));
        }

        #endregion OptionalFields_HP

        #region BuildDictionary

        [Test]
        public void BuildDictionary_WithInput_Null()
        {
            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(null, MAP, TILE_OBJECTS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_NullQuery()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = null
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyQuery()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyName()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ string.Empty, "1,1" }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_WithInput_EmptyCoordinate()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, string.Empty }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict, Is.Empty);
        }

        [Test]
        public void BuildDictionary_InvalidInput()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "test" }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            Assert.Throws<TileObjectInstanceProcessingException>(() => TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS));
        }

        [Test]
        public void BuildDictionary()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "1,1" }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict.Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildDictionary_MultipleObjects()
        {
            MapObjectsConfig config = new MapObjectsConfig()
            {
                Query = new Query()
                {
                    Data = new List<IList<object>>()
                    {
                        new List<object>(){ INPUT_NAME, "1,1" },
                        new List<object>(){ INPUT_NAME, "1,1" },
                        new List<object>(){ INPUT_NAME, "2,1" },
                        new List<object>(){ INPUT_NAME, "1,2" }
                    }
                },
                Name = 0,
                Coordinate = 1
            };

            IDictionary<int, ITileObjectInstance> dict = TileObjectInstance.BuildDictionary(config, MAP, TILE_OBJECTS);
            Assert.That(dict.Count, Is.EqualTo(4));
        }

        #endregion BuildDictionary
    }
}
