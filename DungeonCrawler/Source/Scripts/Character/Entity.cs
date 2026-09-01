namespace DungeonCrawler;

internal enum RaceType : byte { Human, Demon };
internal abstract class Entity
{
    protected Sprite? _sprite;

    protected Entity()
    {
        LoadSprite();
    }

    private void LoadSprite()
    {
        var attribute = GetType().GetCustomAttribute<SpriteAttribute>();

        if (attribute == null)
            return;

        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "XMLs/" + attribute.File);

        _sprite = atlas.CreateSprite(attribute.SpriteName);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Layer layer)
    {
        _sprite?.Draw(spriteBatch, position, layer);
    }
}