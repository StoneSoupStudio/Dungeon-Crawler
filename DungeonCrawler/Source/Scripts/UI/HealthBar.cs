namespace DungeonCrawler;

internal sealed class HealthBar
{
    private readonly Texture2D _pixel;

    public HealthBar()
    {
        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer, Vector2 position, int currentHealth, int maxHealth, Color backColor, Color fillColor)
    {
        int barWidth = Tile.TILE_SIZE - 2;
        int barHeight = 2;

        int healthBarX = (int)position.X + (Tile.TILE_SIZE - barWidth) / 2;
        int healthBarY = (int)position.Y - barHeight - 2;
        float healthPercentage = (float)currentHealth / maxHealth;
        int filledWidth = (int)(barWidth * healthPercentage);

        Vector2 origin = new Vector2((barWidth - 2) / 2, -Tile.TILE_SIZE / 2 + barHeight);

        spriteBatch.Draw(_pixel, position, new Rectangle(healthBarX, healthBarY, barWidth, barHeight), backColor * 0.45f, 0f, origin, 1f, SpriteEffects.None, layer.Depth);
        spriteBatch.Draw(_pixel, position, new Rectangle(healthBarX, healthBarY, filledWidth, barHeight), fillColor, 0f, origin, 1f, SpriteEffects.None, layer.Depth + 0.01f);
    }
}