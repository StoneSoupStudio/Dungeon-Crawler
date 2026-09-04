namespace DungeonCrawler;

internal sealed class FogOfWar
{
    private readonly Dungeon _dungeon;
    private readonly Texture2D _pixel;

    public int SightRange { get; private set; } = 4;

    public FogOfWar(GraphicsDevice graphicsDevice, Dungeon dungeon)
    {
        _dungeon = dungeon;

        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void Update(Vector2 playerPosition)
    {
        HideVisibleTiles();

        int playerX = (int)(playerPosition.X / Tile.TILE_SIZE);
        int playerY = (int)(playerPosition.Y / Tile.TILE_SIZE);

        if (!IsInsideMap(playerX, playerY))
            return;

        SetVisible(playerX, playerY);

        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 1, 0, 0, 1);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 0, 1, 1, 0);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 0, -1, 1, 0);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 1, 0, 0, -1);

        CastLight(playerX, playerY, 1, 1.0f, 0.0f, -1, 0, 0, 1);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 0, -1, -1, 0);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, 0, 1, -1, 0);
        CastLight(playerX, playerY, 1, 1.0f, 0.0f, -1, 0, 0, -1);
    }

    private void HideVisibleTiles()
    {
        for (int x = 0; x < _dungeon.Width; x++)
        {
            for (int y = 0; y < _dungeon.Height; y++)
            {
                Tile tile = _dungeon.Tiles[x, y];

                if (tile.Visibility == TileVisibility.Visible)
                    tile.Visibility = TileVisibility.Explored;
            }
        }
    }

    private void CastLight(
        int centerX, int centerY, int row, float startSlope, float endSlope,
        int xx, int xy, int yx, int yy)
    {
        if (startSlope < endSlope)
            return;

        float newStart = 0.0f;

        for (int distance = row; distance <= SightRange; distance++)
        {
            bool blocked = false;

            int deltaY = -distance;

            for (int deltaX = -distance; deltaX <= 0; deltaX++)
            {
                float leftSlope = (deltaX - 0.5f) / (deltaY + 0.5f);

                float rightSlope = (deltaX + 0.5f) / (deltaY - 0.5f);

                if (startSlope < rightSlope)
                    continue;

                if (endSlope > leftSlope)
                    break;

                int currentX = centerX + deltaX * xx + deltaY * xy;

                int currentY = centerY + deltaX * yx + deltaY * yy;

                if (!IsInsideMap(currentX, currentY))
                    continue;

                float distanceSquared = deltaX * deltaX + deltaY * deltaY;

                bool withinRange = distanceSquared <= SightRange * SightRange;

                if (withinRange)
                    SetVisible(currentX, currentY);

                bool opaque = BlocksVision(currentX, currentY);

                if (blocked)
                {
                    if (opaque)
                    {
                        newStart = rightSlope;
                        continue;
                    }

                    blocked = false;
                    startSlope = newStart;
                }
                else if (opaque)
                {
                    blocked = true;

                    CastLight(centerX, centerY, distance + 1, startSlope, leftSlope, xx, xy, yx, yy);

                    newStart = rightSlope;
                }
            }

            if (blocked)
                break;
        }
    }

    private bool BlocksVision(int x, int y)
    {
        Tile tile = _dungeon.Tiles[x, y];

        return tile.Type == TileType.Wall;
    }

    private void SetVisible(int x, int y)
    {
        if (!IsInsideMap(x, y))
            return;

        _dungeon.Tiles[x, y].Visibility = TileVisibility.Visible;
    }

    private bool IsInsideMap(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _dungeon.Width && y < _dungeon.Height;
    }

    public void DrawFog(SpriteBatch spriteBatch, Layer layer)
    {
        for (int x = 0; x < _dungeon.Width; x++)
        {
            for (int y = 0; y < _dungeon.Height; y++)
            {
                Tile tile = _dungeon.Tiles[x, y];

                if (tile.Visibility == TileVisibility.Visible)
                    continue;

                Vector2 center = new Vector2
                (
                    x * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
                    y * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f
                );

                Color fogColor = tile.Visibility switch
                {
                    TileVisibility.Hidden => Color.Black * 0.95f,
                    TileVisibility.Explored => Color.Black * 0.6f,
                    _ => Color.Transparent
                };

                spriteBatch.Draw(_pixel, center, null, fogColor, 0f, new Vector2(0.5f, 0.5f), new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE), SpriteEffects.None, layer.Depth);
            }
        }
    }
}