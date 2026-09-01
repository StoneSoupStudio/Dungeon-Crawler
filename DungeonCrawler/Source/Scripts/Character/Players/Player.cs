namespace DungeonCrawler;

[Sprite("character-prefabs", "white-man")]
internal class Player : Entity
{
    private readonly PlayerBehavior _behavior;
    public PlayerBehavior Behavior => _behavior;

    private readonly PlayerDB _dB;
    public PlayerDB DB => _dB;

    private readonly Accessory[] _accessories;

    private const byte MAX_ACCESSORY = 10;

    public Player()
    {
        _behavior = new();
        _dB = new();

        _accessories = new Accessory[MAX_ACCESSORY];
    }

    public void Update()
    {
        _behavior.HandlerInput();

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
    }
}