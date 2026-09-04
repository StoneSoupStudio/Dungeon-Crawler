namespace DungeonCrawler;

public struct Slot
{
    private Sprite _sprite;

    public Slot(ContentManager content)
    {
        TextureAtlas atlas = TextureAtlas.FromFile(content, "xmls/gui-prefabs");
        _sprite = atlas.CreateSprite("inventory-slot");
    }

    public void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _sprite.Draw(spriteBatch, new Vector2(100, 100), layer);
    }
}