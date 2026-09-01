namespace DungeonCrawler;

internal abstract class Accessory
{
    private Sprite _sprite;

    private Vector2 _position;

    public EquipType EquipType { get; }

    protected Accessory()
    {
        EquipType = GetEquipType();
        LoadSprite();
    }

    public virtual void Update(Vector2 playerPosition)
    {
        Vector2 basePosition = playerPosition + EquipOffsets.Get(EquipType);

        _position = ChangeOffset(basePosition);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        _sprite?.Draw(spriteBatch, _position, EquipInfo.GetLayer(EquipType));
    }

    public virtual Vector2 ChangeOffset(Vector2 currentPosition)
    {
        return currentPosition;
    }

    private EquipType GetEquipType()
    {
        AutoloadEquipAttribute attribute =
            GetType().GetCustomAttribute<AutoloadEquipAttribute>();

        return attribute?.Type ?? EquipType.None;
    }

    private void LoadSprite()
    {
        SpriteAttribute attribute =
            GetType().GetCustomAttribute<SpriteAttribute>();

        if (attribute == null)
            return;

        TextureAtlas atlas = TextureAtlas.FromFile(
            Core.Content,
            "XMLs/" + attribute.File
        );

        _sprite = atlas.CreateSprite(
            attribute.SpriteName
        );
    }
}

internal static class EquipOffsets
{
    public static Vector2 Get(EquipType type)
    {
        return type switch
        {
            EquipType.Feet => new Vector2(0, 12),
            EquipType.Legs => new Vector2(0, 6),
            EquipType.Body => Vector2.Zero,
            EquipType.Hands => new Vector2(0, -2),
            EquipType.Head => new Vector2(0, -12),
            EquipType.Back => new Vector2(0, 4),

            _ => Vector2.Zero
        };
    }
}

internal static class EquipInfo
{
    public static Layer GetLayer(EquipType type)
    {
        return type switch
        {
            EquipType.Back => Layer.BackLayer,

            EquipType.Feet => Layer.AccessoryLayer,
            EquipType.Legs => Layer.AccessoryLayer,
            EquipType.Body => Layer.AccessoryLayer,

            EquipType.Hands => Layer.AccessoryLayer,
            EquipType.Head => Layer.AccessoryLayer,

            _ => Layer.PlayerLayer
        };
    }
}