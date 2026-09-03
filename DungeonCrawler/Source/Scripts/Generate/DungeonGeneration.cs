namespace DungeonCrawler;

internal sealed class DungeonGeneration
{
    private ushort _width, _height;
    public ushort Width => _width;
    public ushort Height => _height;

    private Tile[,] _tiles;
    public Tile[,] Tiles => _tiles;

    private TextureAtlas _floorAtlas;
    private TextureAtlas _wallAtlas;

    public DungeonGeneration(ContentManager content, ushort width, ushort height)
    {
        _width = width;
        _height = height;

        _tiles = new Tile[width, height];

        _floorAtlas = TextureAtlas.FromFile(content, "xmls/floor-tile-prefabs");
        _wallAtlas = TextureAtlas.FromFile(content, "XMLs/wall-tile-prefabs");

        Generate();
    }

    private void Generate()
    {
        Random rnd = new();

        TextureRegion[] floorTextures = new TextureRegion[9];
        for (int i = 1; i < floorTextures.Length; i++)
            floorTextures[i] = _floorAtlas.GetRegion($"gray-floor-tile-{i}");

        TextureRegion[] wallTextures = new TextureRegion[13];
        for (int i = 1; i < wallTextures.Length; i++)
            wallTextures[i] = _wallAtlas.GetRegion($"gray-wall-tile-{i}");

        // Сначала вся карта становится стеной
        GenerateWalls(wallTextures, rnd);

        // Создаём комнаты
        GenerateRooms(floorTextures, rnd);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in _tiles)
            tile.Draw(spriteBatch);
    }

    private void CreateFloor(
    int x,
    int y,
    TextureRegion[] textures,
    Random rnd)
    {
        TextureRegion floor =
            textures[rnd.Next(1, textures.Length)];

        _tiles[x, y].ChangeTileValue(
            floor,
            TileType.Floor,
            Layer.FloorLayer);
    }

    private void GenerateBorder(TextureRegion[] textureRegion, Random rnd)
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
            _tiles[x, 0].ChangeTileValue(textureRegion[rnd.Next(1, textureRegion.Length)], TileType.Wall, Layer.WallLayer);

        for (int x = 0; x < _tiles.GetLength(0); x++)
            _tiles[x, _tiles.GetLength(1) - 1].ChangeTileValue(textureRegion[rnd.Next(1, textureRegion.Length)], TileType.Wall, Layer.WallLayer);

        for (int y = 0; y < _tiles.GetLength(1); y++)
            _tiles[0, y].ChangeTileValue(textureRegion[rnd.Next(1, textureRegion.Length)], TileType.Wall, Layer.WallLayer);

        for (int y = 0; y < _tiles.GetLength(1); y++)
            _tiles[_tiles.GetLength(0) - 1, y].ChangeTileValue(textureRegion[rnd.Next(1, textureRegion.Length)], TileType.Wall, Layer.WallLayer);
    }

    private void GenerateWalls(TextureRegion[] textures, Random rnd)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TextureRegion wall =
                    textures[rnd.Next(1, textures.Length)];

                _tiles[x, y] = new Tile(
                    wall,
                    new Vector2(
                        x * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
                        y * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f),
                    TileType.Wall,
                    Layer.WallLayer);
            }
        }
    }

    private void GenerateRooms(TextureRegion[] textures, Random rnd)
    {
        const int roomCount = 8;

        Point previousCenter = Point.Zero;

        for (int i = 0; i < roomCount; i++)
        {
            int roomWidth = rnd.Next(5, 10);
            int roomHeight = rnd.Next(5, 8);

            int roomX = rnd.Next(1, _width - roomWidth - 1);
            int roomY = rnd.Next(1, _height - roomHeight - 1);

            Point center = new Point(
                roomX + roomWidth / 2,
                roomY + roomHeight / 2);

            // Комната
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    CreateFloor(x, y, textures, rnd);
                }
            }

            // Соединяем с предыдущей комнатой
            if (i > 0)
                CreateCorridor(previousCenter, center, textures, rnd);

            previousCenter = center;
        }
    }

    private void CreateCorridor(
    Point start,
    Point end,
    TextureRegion[] textures,
    Random rnd)
    {
        int x = start.X;
        int y = start.Y;

        while (x != end.X)
        {
            CreateFloor(x, y, textures, rnd);

            x += Math.Sign(end.X - x);
        }

        while (y != end.Y)
        {
            CreateFloor(x, y, textures, rnd);

            y += Math.Sign(end.Y - y);
        }

        CreateFloor(x, y, textures, rnd);
    }
}