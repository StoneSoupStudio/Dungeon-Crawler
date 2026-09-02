namespace DungeonCrawler;

public enum StateType : byte { Idle, Move, Attack };
[Sprite("character-prefabs", "gnome-mage")]
internal sealed class GnomeMage : Entity, IEnemy
{
    private Vector2 _position;

    public GnomeMage()
    {
        _position = new Vector2(4 * Tile.TILE_SIZE, 10 * Tile.TILE_SIZE);
    }

    private GnomeMage(GnomeMage gnomeMage)
    {
        _position = gnomeMage._position + new Vector2(10, 10);
    }

    public void Update(Player player)
    {
        if (Vector2.Distance(_position, player.Behavior.Position) <= 3 * Tile.TILE_SIZE)
        {
            Vector2 direction = Vector2.Normalize(player.Behavior.Position - _position);
            _position += direction * 1f;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch, _position, Layer.EntityLayer);
    }

    public IEnemy Clone() => new GnomeMage(this);
}