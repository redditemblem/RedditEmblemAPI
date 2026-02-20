using NSubstitute;
using RedditEmblemAPI.Models.Configuration.Common;
using RedditEmblemAPI.Models.Configuration.Map;
using RedditEmblemAPI.Models.Exceptions.Processing;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Exceptions.Validation;
using RedditEmblemAPI.Models.Output.Map;
using RedditEmblemAPI.Models.Output.Map.Tiles;
using RedditEmblemAPI.Models.Output.System;

namespace UnitTests.Models.Map
{
    internal class MapObjTests
    {
        #region Constants

        private const string INPUT_MAP_SWITCH_ON = "On";

        private const string INPUT_IMAGE_URL_1 = "http://www.fakewebsite.com/map_image_1.png";
        private const string INPUT_IMAGE_URL_2 = "http://www.fakewebsite.com/map_image_2.png";
        private const string INPUT_IMAGE_URL_3 = "http://www.fakewebsite.com/map_image_3.png";

        private const string PLAINS = "Plains";
        private const string PATH = "Path";
        private const string TREES = "Trees";
        private const string WARP_IN = "Warp In";
        private const string WARP_OUT = "Warp Out";
        private const string WARP_DUAL = "Warp Dual";

        #endregion Constants

        #region SetUp

        private IImageLoader ImageLoader;
        private IDictionary<string, ITerrainType> TERRAIN_TYPES;
        private IDictionary<string, ITileObject> TILE_OBJECTS;

        [SetUp]
        public void SetUp()
        {
            this.ImageLoader = Substitute.For<IImageLoader>();
            this.TERRAIN_TYPES = SetUp_TerrainTypes();
            this.TILE_OBJECTS = SetUp_TileObjects();
        }

        private IDictionary<string, ITerrainType> SetUp_TerrainTypes()
        {
            IDictionary<string, ITerrainType> terrainTypes = new Dictionary<string, ITerrainType>();

            ITerrainType terrain1 = Substitute.For<ITerrainType>();
            terrain1.Name.Returns(PLAINS);

            ITerrainType terrain2 = Substitute.For<ITerrainType>();
            terrain2.Name.Returns(PATH);

            ITerrainType terrain3 = Substitute.For<ITerrainType>();
            terrain3.Name.Returns(TREES);

            ITerrainType terrain4 = Substitute.For<ITerrainType>();
            terrain4.Name.Returns(WARP_IN);
            terrain4.WarpType.Returns(WarpType.Entrance);

            ITerrainType terrain5 = Substitute.For<ITerrainType>();
            terrain5.Name.Returns(WARP_OUT);
            terrain5.WarpType.Returns(WarpType.Exit);

            ITerrainType terrain6 = Substitute.For<ITerrainType>();
            terrain6.Name.Returns(WARP_DUAL);
            terrain6.WarpType.Returns(WarpType.Dual);

            terrainTypes.Add(PLAINS, terrain1);
            terrainTypes.Add(PATH, terrain2);
            terrainTypes.Add(TREES, terrain3);
            terrainTypes.Add(WARP_IN, terrain4);
            terrainTypes.Add(WARP_OUT, terrain5);
            terrainTypes.Add(WARP_DUAL, terrain6);

            return terrainTypes;
        }

        private IDictionary<string, ITileObject> SetUp_TileObjects()
        {
            IDictionary<string, ITileObject> tileObjects = new Dictionary<string, ITileObject>();

            string tileObj1Name = "Tile Object 1";
            ITileObject tileObj1 = Substitute.For<ITileObject>();
            tileObj1.Name.Returns(tileObj1Name);
            tileObj1.Size.Returns(1);

            tileObjects.Add(tileObj1Name, tileObj1);

            return tileObjects;
        }

        #endregion SetUp

        #region Constructor

        [Test]
        public void Constructor_WithInput_Null()
        {
            Assert.Throws<NullReferenceException>(() => new MapObj(null, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_WithInput_NullQuery()
        {
            MapConfig config = new MapConfig()
            {
                MapControls = new MapControlsConfig()
                {
                    Query = null
                }
            };

            Assert.Throws<NullReferenceException>(() => new MapObj(null, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("test")]
        [TestCase("Off")]
        public void Constructor_MapIsDisabled(string input)
        {
            MapConfig config = new MapConfig()
            {
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ input }
                        }
                    },
                    MapSwitch = 0
                }
            };

            Assert.Throws<MapDataLockedException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_MapImageURLsNotFound()
        {
            MapConfig config = new MapConfig()
            {
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                }
            };

            Assert.Throws<MapImageURLsNotFoundException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));
        }

        [Test]
        public void Constructor_EmptyMapTileData()
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig(){ TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ }
                        }
                    }
                }
            };

            //5 x 5 map size
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 80;
                s[2] = 80;
            });

            MapProcessingException ex = 
                Assert.Throws<MapProcessingException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));

            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.TypeOf<UnexpectedMapHeightException>());
        }

        [Test]
        public void Constructor_TooManyMapTileRows()
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ }
                        }
                    }
                }
            };

            //5 x 5 map size
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 80;
                s[2] = 80;
            });

            MapProcessingException ex =
                Assert.Throws<MapProcessingException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));

            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.TypeOf<UnexpectedMapHeightException>());
        }

        [Test]
        public void Constructor_MapTileRowsTooShort()
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ },
                            new List<object>(){ }
                        }
                    }
                }
            };

            //5 x 5 map size
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 80;
                s[2] = 80;
            });

            MapProcessingException ex =
                Assert.Throws<MapProcessingException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));

            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.TypeOf<UnexpectedMapWidthException>());
        }

        #region Segment Creation

        [Test]
        public void Constructor_SingleSegment_3x3()
        {
            int rowCount = 3;
            int columnCount = 3;
            ITerrainType plains = TERRAIN_TYPES[PLAINS];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));

            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(rowCount), "The segment should contain 3 rows.");

            //Validate each tile individually
            for(int r = 0; r < rowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(columnCount), "All rows within the segment should have 3 values.");

                for (int c = 0; c < columnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c+1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r+1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }
        }

        [Test]
        public void Constructor_SingleSegment_2x5()
        {
            int rowCount = 5;
            int columnCount = 2;

            ITerrainType plains = TERRAIN_TYPES[PLAINS];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 80;
                s[2] = 32;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));

            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(rowCount), "The segment should contain 5 rows.");

            //Validate each tile individually
            for (int r = 0; r < rowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(columnCount), "All rows within the segment should have 2 values.");

                for (int c = 0; c < columnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }
        }

        [Test]
        public void Constructor_SingleSegment_5x2()
        {
            int rowCount = 2;
            int columnCount = 5;

            ITerrainType plains = TERRAIN_TYPES[PLAINS];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, PLAINS, PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS, PLAINS, PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 80;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));

            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(rowCount), "The segment should contain 2 rows.");

            //Validate each tile individually
            for (int r = 0; r < rowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(columnCount), "All rows within the segment should have 5 values.");

                for (int c = 0; c < columnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }
        }

        [Test]
        public void Constructor_TwoSegments_SameSize_2x2()
        {
            int rowCount = 2;
            int columnCount = 2;
            ITerrainType plains = TERRAIN_TYPES[PLAINS];
            ITerrainType path = TERRAIN_TYPES[PATH];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1, INPUT_IMAGE_URL_2 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1, 2 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, "", "", PATH, PATH },
                            new List<object>(){ PLAINS, PLAINS, "", "", PATH, PATH }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(Arg.Any<string>(), out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(2));

            //Validate segment 1
            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(rowCount), "Segment 1 should contain 2 rows.");

            for (int r = 0; r < rowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(columnCount), "All rows within segment 1 should have 2 values.");

                for (int c = 0; c < columnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }

            //Validate segment 2
            tiles = map.Segments[1].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(rowCount), "Segment 2 should contain 2 rows.");

            for (int r = 0; r < rowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(columnCount), "All rows within segment 2 should have 2 values.");

                for (int c = 0; c < columnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 3), "Segment 2's horizontal coordinates should be offset by segment 1.");
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(path));
                }
            }
        }

        [Test]
        public void Constructor_TwoSegments_DifferentSizes_3x3_2x2()
        {
            int seg1RowCount = 3;
            int seg1ColumnCount = 3;

            int seg2RowCount = 2;
            int seg2ColumnCount = 2;

            ITerrainType plains = TERRAIN_TYPES[PLAINS];
            ITerrainType path = TERRAIN_TYPES[PATH];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1, INPUT_IMAGE_URL_2 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1, 2 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, PLAINS, "", "", PATH, PATH },
                            new List<object>(){ PLAINS, PLAINS, PLAINS, "", "", PATH, PATH },
                            new List<object>(){ PLAINS, PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_2, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(2));

            //Validate segment 1
            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg1RowCount), "Segment 1 should contain 3 rows.");

            for (int r = 0; r < seg1RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg1ColumnCount), "All rows within segment 1 should have 3 values.");

                for (int c = 0; c < seg1ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }

            //Validate segment 2
            tiles = map.Segments[1].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg2RowCount), "Segment 2 should contain 2 rows.");

            for (int r = 0; r < seg2RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg2ColumnCount), "All rows within segment 2 should have 2 values.");

                for (int c = 0; c < seg2ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 4), "Segment 2's horizontal coordinates should be offset by segment 1.");
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(path));
                }
            }
        }

        [Test]
        public void Constructor_TwoSegments_DifferentSizes_2x2_3x3()
        {
            int seg1RowCount = 2;
            int seg1ColumnCount = 2;

            int seg2RowCount = 3;
            int seg2ColumnCount = 3;

            ITerrainType plains = TERRAIN_TYPES[PLAINS];
            ITerrainType path = TERRAIN_TYPES[PATH];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1, INPUT_IMAGE_URL_2 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1, 2 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, "", "", PATH, PATH, PATH },
                            new List<object>(){ PLAINS, PLAINS, "", "", PATH, PATH, PATH },
                            new List<object>(){ "", "",         "", "", PATH, PATH, PATH }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_2, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(2));

            //Validate segment 1
            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg1RowCount), "Segment 1 should contain 2 rows.");

            for (int r = 0; r < seg1RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg1ColumnCount), "All rows within segment 1 should have 2 values.");

                for (int c = 0; c < seg1ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }

            //Validate segment 2
            tiles = map.Segments[1].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg2RowCount), "Segment 2 should contain 3 rows.");

            for (int r = 0; r < seg2RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg2ColumnCount), "All rows within segment 2 should have 3 values.");

                for (int c = 0; c < seg2ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 3), "Segment 2's horizontal coordinates should be offset by segment 1.");
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(path));
                }
            }
        }

        [Test]
        public void Constructor_ThreeSegments_DifferentSizes_2x2_3x3_4x5()
        {
            int seg1RowCount = 2;
            int seg1ColumnCount = 2;

            int seg2RowCount = 3;
            int seg2ColumnCount = 3;

            int seg3RowCount = 5;
            int seg3ColumnCount = 4;

            ITerrainType plains = TERRAIN_TYPES[PLAINS];
            ITerrainType path = TERRAIN_TYPES[PATH];
            ITerrainType trees = TERRAIN_TYPES[TREES];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1, INPUT_IMAGE_URL_2, INPUT_IMAGE_URL_3 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1, 2, 3 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS, PATH, PATH, PATH, TREES, TREES, TREES, TREES },
                            new List<object>(){ PLAINS, PLAINS, PATH, PATH, PATH, TREES, TREES, TREES, TREES },
                            new List<object>(){ "", "",         PATH, PATH, PATH, TREES, TREES, TREES, TREES },
                            new List<object>(){ "", "",         "", "", "",       TREES, TREES, TREES, TREES },
                            new List<object>(){ "", "",         "", "", "",       TREES, TREES, TREES, TREES }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_2, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });
            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_3, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 80;
                s[2] = 64;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(3));

            //Validate segment 1
            ITile[][] tiles = map.Segments[0].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg1RowCount), "Segment 1 should contain 2 rows.");

            for (int r = 0; r < seg1RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg1ColumnCount), "All rows within segment 1 should have 2 values.");

                for (int c = 0; c < seg1ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 1));
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(plains));
                }
            }

            //Validate segment 2
            tiles = map.Segments[1].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg2RowCount), "Segment 2 should contain 3 rows.");

            for (int r = 0; r < seg2RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg2ColumnCount), "All rows within segment 2 should have 3 values.");

                for (int c = 0; c < seg2ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 3), "Segment 2's horizontal coordinates should be offset by segment 1.");
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(path));
                }
            }

            //Validate segment 3
            tiles = map.Segments[2].Tiles;
            Assert.That(tiles.Count(), Is.EqualTo(seg3RowCount), "Segment 3 should contain 5 rows.");

            for (int r = 0; r < seg3RowCount; r++)
            {
                Assert.That(tiles[r].Count(), Is.EqualTo(seg3ColumnCount), "All rows within segment 3 should have 4 values.");

                for (int c = 0; c < seg3ColumnCount; c++)
                {
                    ITile tile = tiles[r][c];

                    Assert.That(tile, Is.Not.Null);
                    Assert.That(tile.Coordinate.X, Is.EqualTo(c + 6), "Segment 3's horizontal coordinates should be offset by segments 1 and 2.");
                    Assert.That(tile.Coordinate.Y, Is.EqualTo(r + 1));
                    Assert.That(tiles[r][c].TerrainType, Is.EqualTo(trees));
                }
            }
        }

        #endregion Segment Creation

        #region Tile Object Creation

        [Test]
        public void Constructor_SingleSegment_WithTileObjects()
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS }
                        }
                    }
                },
                MapObjects = new MapObjectsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ "Tile Object 1", "2,2" }
                        }
                    },
                    Name = 0,
                    Coordinate = 1
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));

            ITile[][] tiles = map.Segments[0].Tiles;

            ITile tile1 = tiles[0][0];
            ITile tile2 = tiles[0][1];
            ITile tile3 = tiles[1][0];
            ITile tile4 = tiles[1][1];

            Assert.That(tile1, Is.Not.Null);
            Assert.That(tile2, Is.Not.Null);
            Assert.That(tile3, Is.Not.Null);
            Assert.That(tile4, Is.Not.Null);

            Assert.That(tile1.TileObjects, Is.Empty);
            Assert.That(tile2.TileObjects, Is.Empty);
            Assert.That(tile3.TileObjects, Is.Empty);
            Assert.That(tile4.TileObjects.Count(), Is.EqualTo(1));
        }

        #endregion Tile Object Creation

        #region Warp Groups

        [Test]
        public void Constructor_TerrainTypeIsNotWarpError()
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ $"{PLAINS} (1)", PLAINS },
                            new List<object>(){ PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            MapProcessingException ex = 
                Assert.Throws<MapProcessingException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));

            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.TypeOf<TerrainTypeNotConfiguredAsWarpException>());
        }

        [TestCase(WARP_IN)] //only entrance
        [TestCase(WARP_OUT)] //only exit
        [TestCase(WARP_DUAL)] //entrance + exit, but only one tile
        public void Constructor_InvalidWarpGroups(string terrainType)
        {
            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ $"{terrainType} (1)", PLAINS },
                            new List<object>(){ PLAINS, PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            MapProcessingException ex =
                Assert.Throws<MapProcessingException>(() => new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS));

            Assert.That(ex.InnerException, Is.Not.Null);
            Assert.That(ex.InnerException, Is.TypeOf<InvalidWarpGroupException>());
        }

        [Test]
        public void Constructor_SingleSegment_1WarpGroup()
        {
            ITerrainType warpIn = TERRAIN_TYPES[WARP_IN];
            ITerrainType warpOut = TERRAIN_TYPES[WARP_OUT];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ $"{WARP_IN} (1)", PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS, PLAINS },
                            new List<object>(){ PLAINS, PLAINS, $"{WARP_OUT} (1)" }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));

            ITile tile1 = map.Segments[0].Tiles[0][0];
            ITile tile2 = map.Segments[0].Tiles[2][2];

            Assert.That(tile1.TerrainType, Is.EqualTo(warpIn));
            Assert.That(tile1.WarpData.WarpGroupNumber, Is.EqualTo(1));
            Assert.That(tile2.TerrainType, Is.EqualTo(warpOut));
            Assert.That(tile2.WarpData.WarpGroupNumber, Is.EqualTo(1));

            List<ITile> expected = new List<ITile>() { tile1, tile2 };
            Assert.That(tile1.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile2.WarpData.WarpGroup, Is.EquivalentTo(expected));
        }

        [Test]
        public void Constructor_SingleSegment_2WarpGroups()
        {
            ITerrainType warpIn = TERRAIN_TYPES[WARP_IN];
            ITerrainType warpOut = TERRAIN_TYPES[WARP_OUT];
            ITerrainType warpDual = TERRAIN_TYPES[WARP_DUAL];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ $"{WARP_IN} (1)", PLAINS, $"{WARP_OUT} (2)" },
                            new List<object>(){ PLAINS, PLAINS, $"{WARP_DUAL} (2)" },
                            new List<object>(){ $"{WARP_IN} (2)", PLAINS, $"{WARP_OUT} (1)" }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 48;
                s[2] = 48;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(1));
            
            IMapSegment segment = map.Segments[0];
            ITile tile1 = segment.Tiles[0][0];
            ITile tile2 = segment.Tiles[2][2];

            Assert.That(tile1.TerrainType, Is.EqualTo(warpIn));
            Assert.That(tile1.WarpData.WarpGroupNumber, Is.EqualTo(1));
            Assert.That(tile2.TerrainType, Is.EqualTo(warpOut));
            Assert.That(tile2.WarpData.WarpGroupNumber, Is.EqualTo(1));

            List<ITile> expected = new List<ITile>() { tile1, tile2 };
            Assert.That(tile1.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile2.WarpData.WarpGroup, Is.EquivalentTo(expected));

            ITile tile3 = segment.Tiles[2][0];
            ITile tile4 = segment.Tiles[0][2];
            ITile tile5 = segment.Tiles[1][2];

            Assert.That(tile3.TerrainType, Is.EqualTo(warpIn));
            Assert.That(tile3.WarpData.WarpGroupNumber, Is.EqualTo(2));
            Assert.That(tile4.TerrainType, Is.EqualTo(warpOut));
            Assert.That(tile4.WarpData.WarpGroupNumber, Is.EqualTo(2));
            Assert.That(tile5.TerrainType, Is.EqualTo(warpDual));
            Assert.That(tile5.WarpData.WarpGroupNumber, Is.EqualTo(2));

            expected = new List<ITile>() { tile3, tile4, tile5 };
            Assert.That(tile3.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile4.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile5.WarpData.WarpGroup, Is.EquivalentTo(expected));
        }

        [Test]
        public void Constructor_TwoSegments_2WarpGroups()
        {
            ITerrainType warpIn = TERRAIN_TYPES[WARP_IN];
            ITerrainType warpOut = TERRAIN_TYPES[WARP_OUT];
            ITerrainType warpDual = TERRAIN_TYPES[WARP_DUAL];

            MapConfig config = new MapConfig()
            {
                Constants = new MapConstantsConfig() { TileSize = 16 },
                MapControls = new MapControlsConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ INPUT_MAP_SWITCH_ON, INPUT_IMAGE_URL_1, INPUT_IMAGE_URL_2 }
                        }
                    },
                    MapSwitch = 0,
                    MapImageURLs = new List<int> { 1, 2 }
                },
                MapTiles = new MapTilesConfig()
                {
                    Query = new Query()
                    {
                        Data = new List<IList<object>>()
                        {
                            new List<object>(){ $"{WARP_IN} (1)", PLAINS, "", PLAINS, $"{WARP_IN} (2)" },
                            new List<object>(){ PLAINS, $"{WARP_OUT} (2)", "", $"{WARP_OUT} (1)", PLAINS }
                        }
                    }
                }
            };

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_1, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            this.ImageLoader.When(s => s.GetImageDimensionsByUrl(INPUT_IMAGE_URL_2, out Arg.Any<int>(), out Arg.Any<int>())).Do(s =>
            {
                s[1] = 32;
                s[2] = 32;
            });

            IMapObj map = new MapObj(config, this.ImageLoader, TERRAIN_TYPES, TILE_OBJECTS);

            Assert.That(map.Segments.Count(), Is.EqualTo(2));

            IMapSegment segment1 = map.Segments[0];
            ITile tile1 = segment1.Tiles[0][0];
            ITile tile2 = segment1.Tiles[1][1];

            Assert.That(tile1.TerrainType, Is.EqualTo(warpIn));
            Assert.That(tile1.WarpData.WarpGroupNumber, Is.EqualTo(1));
            Assert.That(tile2.TerrainType, Is.EqualTo(warpOut));
            Assert.That(tile2.WarpData.WarpGroupNumber, Is.EqualTo(2));

            IMapSegment segment2 = map.Segments[1];
            ITile tile3 = segment2.Tiles[0][1];
            ITile tile4 = segment2.Tiles[1][0];

            Assert.That(tile3.TerrainType, Is.EqualTo(warpIn));
            Assert.That(tile3.WarpData.WarpGroupNumber, Is.EqualTo(2));
            Assert.That(tile4.TerrainType, Is.EqualTo(warpOut));
            Assert.That(tile4.WarpData.WarpGroupNumber, Is.EqualTo(1));

            List<ITile> expected = new List<ITile>() { tile1, tile4 };
            Assert.That(tile1.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile4.WarpData.WarpGroup, Is.EquivalentTo(expected));

            expected = new List<ITile>() { tile2, tile3 };
            Assert.That(tile2.WarpData.WarpGroup, Is.EquivalentTo(expected));
            Assert.That(tile3.WarpData.WarpGroup, Is.EquivalentTo(expected));
        }

        #endregion Warp Groups

        #endregion Constructor
    }
}
