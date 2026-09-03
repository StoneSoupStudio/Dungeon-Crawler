namespace DungeonCrawler;

internal sealed class Canvas
{
    private const ushort BACKGROUND_WIDTH = 9 * Tile.TILE_SIZE;
    private const byte OFFSET_X = 16;

    private Texture2D _pixel;

    private Vector2 _position;
    private Rectangle _backgroundSourceRect;

    private SpriteFont _uiFont;

    public Canvas(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _position = new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, 0);

        _backgroundSourceRect = new Rectangle(0, 0, BACKGROUND_WIDTH, Game.SCREEN_HEIGHT);
        _uiFont = content.Load<SpriteFont>("fonts/ui");
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer, Player player)
    {
        spriteBatch.Draw(_pixel, _position, _backgroundSourceRect, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer.Depth);

        spriteBatch.DrawString(_uiFont, player.DB.Name, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, player.DB.Race.ToString(), new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 1), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.DrawString(_uiFont, "Health: " + player.DB.CurrentHealth + "/" + player.DB.MaxHealth, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 3), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "Mana: 50/50", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 4), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.Draw(_pixel, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 2.2f, OFFSET_X * 3), new Rectangle(0, 0, 12 * 13, 14), Color.Green, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.Draw(_pixel, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 2.2f, OFFSET_X * 4), new Rectangle(0, 0, 12 * 13, 14), Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.DrawString(_uiFont, "STR: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 6), Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "DEX: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 7), Color.Green, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "CON: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 8), Color.OrangeRed, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.DrawString(_uiFont, "INT: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 4f, OFFSET_X * 6), Color.CornflowerBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "WIS: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 4f, OFFSET_X * 7), Color.Purple, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "CHA: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 4f, OFFSET_X * 8), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.DrawString(_uiFont, "LVL: 20", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 10), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.DrawString(_uiFont, "EXP: 999/999", new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 4f, OFFSET_X * 10), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.Draw(_pixel, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH + BACKGROUND_WIDTH / 1.58f, OFFSET_X * 10), new Rectangle(0, 0, 12 * 9 - 3, 14), Color.PaleGoldenrod, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        spriteBatch.Draw(_pixel, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 12), new Rectangle(0, 0, BACKGROUND_WIDTH, 6 * Tile.TILE_SIZE - 1), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        spriteBatch.Draw(_pixel, new Vector2(Game.SCREEN_WIDTH - BACKGROUND_WIDTH, OFFSET_X * 24), new Rectangle(0, 0, BACKGROUND_WIDTH, 7 * Tile.TILE_SIZE), Color.Purple, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

    }
}