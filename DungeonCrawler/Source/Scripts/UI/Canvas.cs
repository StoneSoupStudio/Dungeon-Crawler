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
        _pixel.SetData(new[] { Color.Black });

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
    }
}