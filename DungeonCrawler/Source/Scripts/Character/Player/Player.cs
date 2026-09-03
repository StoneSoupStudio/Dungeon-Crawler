namespace DungeonCrawler;

[Sprite("character-prefabs", "white-man")]
internal class Player : Entity
{
    private readonly PlayerBehavior _behavior;
    public PlayerBehavior Behavior => _behavior;

    private readonly PlayerDB _dB;
    public PlayerDB DB => _dB;

    private readonly Accessory[] _accessories;

    private readonly Texture2D _pixel;

    private const byte MAX_ACCESSORY = 10;

    public Player(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _behavior = new();
        _dB = new();

        _accessories = new Accessory[MAX_ACCESSORY];
    }

    public void Update(DungeonGeneration dungeon)
    {
        _behavior.Move(dungeon);

        foreach (Accessory accessory in _accessories)
        {
            accessory?.Update(_behavior.Position);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch, _behavior.Position, Layer.PlayerLayer);

        foreach (Accessory accessory in _accessories)
        {
            accessory?.Draw(spriteBatch);
        }

        DrawHealthBar(spriteBatch, Layer.GUILayer);
    }

    private void DrawHealthBar(SpriteBatch spriteBatch, Layer layer)
    {
        int barWidth = Tile.TILE_SIZE - 2;
        int barHeight = 2;

        int healthBarX = (int)_behavior.Position.X + (Tile.TILE_SIZE - barWidth) / 2;
        int healthBarY = (int)_behavior.Position.Y - barHeight - 2;
        float healthPercentage = (float)_dB.CurrentHealth / _dB.MaxHealth;
        int filledWidth = (int)(barWidth * healthPercentage);

        Vector2 origin = new Vector2(barWidth / 2, -Tile.TILE_SIZE / 2 + barHeight);

        spriteBatch.Draw(_pixel, _behavior.Position, new Rectangle(healthBarX, healthBarY, barWidth, barHeight), Color.Red * 0.45f, 0f, origin, 1f, SpriteEffects.None, layer.Depth);
        spriteBatch.Draw(_pixel, _behavior.Position, new Rectangle(healthBarX, healthBarY, filledWidth, barHeight), Color.Green, 0f, origin, 1f, SpriteEffects.None, layer.Depth + 0.01f);
    }
}