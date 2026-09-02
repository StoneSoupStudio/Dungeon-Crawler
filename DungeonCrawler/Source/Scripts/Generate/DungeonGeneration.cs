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

        GenerateFloor(floorTextures, rnd);
        GenerateBorder(wallTextures, rnd);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in _tiles)
            tile.Draw(spriteBatch);
    }

    private void GenerateFloor(TextureRegion[] textureRegion, Random rnd)
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                TextureRegion floor = textureRegion[rnd.Next(1, textureRegion.Length)];
                _tiles[x, y] = new Tile(floor, new Vector2(x * Tile.TILE_SIZE, y * Tile.TILE_SIZE), TileType.Floor, Layer.FloorLayer);
            }
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
}