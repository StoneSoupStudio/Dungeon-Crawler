namespace DungeonCrawler;

public enum TileType : byte { Error = byte.MaxValue, None = 0, Floor = 1, Wall = 2, Door = 3 };
[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct Tile
{
    public const byte TILE_SIZE = 32;

    private readonly TextureRegion _texture;
    private Vector2 _position;
    private Rectangle _sourceRectangle;

    public Tile(TextureRegion texture, Vector2 position)
    {
        this._texture = texture;
        _position = position;

        _sourceRectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
    }

    public readonly void Draw(SpriteBatch spriteBatch, Layer layer)
    {
        _texture.Draw(spriteBatch, _position, Color.White, 0f, CentrePivot(_sourceRectangle), Vector2.One, SpriteEffects.None, layer.Depth);
    }

    private static Vector2 CentrePivot(Rectangle rectangle)
    {
        return new Vector2(rectangle.Width, rectangle.Height) * 0.5f;
    }
}