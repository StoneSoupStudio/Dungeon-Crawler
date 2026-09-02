namespace DungeonCrawler;

public enum TileType : byte { Error = byte.MaxValue, None = 0, Floor, Wall, Door, Furniture, Water, Ladder };
[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct Tile
{
    public const byte TILE_SIZE = 32;

    private TextureRegion _texture;
    private Vector2 _position;
    private Rectangle _sourceRectangle;

    public TileType Type { get; private set; }
    public Layer Layer { get; private set; }

    public readonly bool IsWalkable => Type == TileType.Floor || Type == TileType.Door;

    public Tile(TextureRegion texture, Vector2 position, TileType type, Layer layer)
    {
        this._texture = texture;
        this._position = position;

        _sourceRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        this.Type = type;
        this.Layer = layer;
    }

    public readonly void Draw(SpriteBatch spriteBatch)
    {
        _texture.Draw(spriteBatch, _position, Color.White, 0f, CentrePivot(_sourceRectangle), Vector2.One, SpriteEffects.None, Layer.Depth);
    }

    public void ChangeTileValue(TextureRegion newTexture, TileType newType, Layer layer)
    {
        this._texture = newTexture;
        Type = newType;
        this.Layer = layer;
    }

    private static Vector2 CentrePivot(Rectangle rectangle)
    {
        return new Vector2(rectangle.Width, rectangle.Height) * 0.5f;
    }
}