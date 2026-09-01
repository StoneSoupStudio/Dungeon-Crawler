namespace DungeonCrawler;

internal sealed class DungeonGeneration
{
    private ushort _width, _height;
    public ushort Width => _width;
    public ushort Height => _height;

    private Tile[] _tiles;

    private TextureAtlas _floorAtlas;
    private TextureAtlas _wallAtlas;

    public DungeonGeneration(ContentManager content, ushort width, ushort height)
    {
        _width = width;
        _height = height;

        _tiles = new Tile[width * height];

        _floorAtlas = TextureAtlas.FromFile(content, "xmls/floor-tile-prefabs");
        _wallAtlas = TextureAtlas.FromFile(content, "XMLs/wall-tile-prefabs");

        Generate();
    }

    private void Generate()
    {
        Random rnd = new Random();

        TextureRegion[] floorTextures = new TextureRegion[9];
        for (int i = 1; i < floorTextures.Length; i++)
            floorTextures[i] = _floorAtlas.GetRegion($"gray-floor-tile-{i}");

        TextureRegion[] wallTextures = new TextureRegion[13];
        for (int i = 1; i < wallTextures.Length; i++)
            wallTextures[i] = _wallAtlas.GetRegion($"gray-wall-tile-{i}");

        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
            {
                TextureRegion floor = floorTextures[rnd.Next(1, floorTextures.Length)];
                _tiles[x + _width * y] = new Tile(floor, new Vector2(x * Tile.TILE_SIZE, y * Tile.TILE_SIZE));
            }

        for (int x = 0; x < _width; x++)
        {
            TextureRegion wall1 = wallTextures[rnd.Next(1, wallTextures.Length)];
            _tiles[x + _width] = new Tile(wall1, new Vector2(x * Tile.TILE_SIZE, 0 * Tile.TILE_SIZE));
        }

        for (int x = 0; x < _width; x++)
        {
            TextureRegion wall1 = wallTextures[rnd.Next(1, wallTextures.Length)];
            _tiles[x + _height] = new Tile(wall1, new Vector2(x * Tile.TILE_SIZE, _height * Tile.TILE_SIZE));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Tile tile in _tiles)
            tile.Draw(spriteBatch, Layer.FloorLayer);
    }
}