namespace DungeonCrawler;

internal sealed class Minimap
{
    private readonly Dungeon _dungeon;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly Texture2D _pixel;
    private RenderTarget2D _renderTarget;

    public int TilePixelSize { get; set; } = 3;
    public Point ScreenPosition { get; init; } = new Point(Game.SCREEN_WIDTH - Canvas.BACKGROUND_WIDTH + Canvas.OFFSET_X * 4, Tile.TILE_SIZE * 6);

    private bool _isDirty = true;

    public Minimap(GraphicsDevice graphicsDevice, Dungeon dungeon)
    {
        _graphicsDevice = graphicsDevice;
        _dungeon = dungeon;

        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _renderTarget = new RenderTarget2D(
            graphicsDevice,
            dungeon.Width * TilePixelSize,
            dungeon.Height * TilePixelSize);
    }

    public void MarkDirty() => _isDirty = true;

    private void RebuildTexture(SpriteBatch spriteBatch)
    {
        _graphicsDevice.SetRenderTarget(_renderTarget);
        _graphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        for (int x = 0; x < _dungeon.Width; x++)
        {
            for (int y = 0; y < _dungeon.Height; y++)
            {
                Tile tile = _dungeon.Tiles[x, y];

                if (tile.Visibility == TileVisibility.Hidden)
                    continue;

                Color color = GetTileColor(tile);

                Rectangle destination = new Rectangle(
                    x * TilePixelSize,
                    y * TilePixelSize,
                    TilePixelSize,
                    TilePixelSize);

                spriteBatch.Draw(_pixel, destination, color);
            }
        }

        spriteBatch.End();

        _graphicsDevice.SetRenderTarget(null);
        _isDirty = false;
    }

    public void Update(SpriteBatch spriteBatch)
    {
        if (_isDirty)
            RebuildTexture(spriteBatch);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
    {
        spriteBatch.Draw(
            _renderTarget,
            ScreenPosition.ToVector2(),
            null,
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0.99f);

        int playerTileX = (int)(playerPosition.X / Tile.TILE_SIZE);
        int playerTileY = (int)(playerPosition.Y / Tile.TILE_SIZE);

        Rectangle playerMarker = new Rectangle(
            ScreenPosition.X + playerTileX * TilePixelSize - 1,
            ScreenPosition.Y + playerTileY * TilePixelSize - 1,
            TilePixelSize + 2,
            TilePixelSize + 2);

        spriteBatch.Draw(
            _pixel,
            playerMarker,      // destinationRectangle — задаёт и позицию, и размер сразу
            null,               // sourceRectangle
            Color.Yellow,
            0f,
            Vector2.Zero,       // origin — здесь трактуется в пикселях текстуры _pixel (1x1)
            SpriteEffects.None,
            1f);
    }

    private Color GetTileColor(Tile tile)
    {
        Color baseColor = tile.Type switch
        {
            TileType.Wall => Color.Gray,
            TileType.Floor => Color.SaddleBrown,
            TileType.Door => Color.SandyBrown,
            TileType.Water => Color.CornflowerBlue,
            TileType.Ladder => Color.Goldenrod,
            _ => Color.Transparent
        };

        if (tile.Visibility == TileVisibility.Explored)
            baseColor *= 0.5f;

        return baseColor;
    }
}