namespace DungeonCrawler;

[Sprite("character-prefabs", "white-man")]
internal class Player : Entity
{
    private readonly string _name = "Player";
    public string Name => _name;

    private readonly PlayerBehavior _behavior;
    public PlayerBehavior Behavior => _behavior;

    private readonly StatDB _dB;
    public StatDB DB => _dB;

    private readonly Accessory[] _accessories;

    private readonly Texture2D _pixel;

    private const byte MAX_ACCESSORY = 10;
    private HealthBar _healthBar;

    private Inventory _inventory;

    public Player(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _healthBar = new();

        _behavior = new();
        _dB = new(RaceType.Human);

        _accessories = new Accessory[MAX_ACCESSORY];
        _inventory = new Inventory(content);
    }

    public void Update(Dungeon dungeon, GameTime gameTime)
    {
        _behavior.Move(gameTime, dungeon);

        foreach (Accessory accessory in _accessories)
        {
            accessory?.Update(_behavior.Position);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch, _behavior.Position, Layer.PlayerLayer);
        _inventory.Draw(spriteBatch);

        foreach (Accessory accessory in _accessories)
        {
            accessory?.Draw(spriteBatch);
        }

        _healthBar.Draw(spriteBatch, Layer.GUILayer, _behavior.Position, _dB.CurrentHealth, _dB.MaxHealth, Color.Red, Color.Green);
    }
}