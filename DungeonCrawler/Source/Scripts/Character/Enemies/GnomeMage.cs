namespace DungeonCrawler;

public enum StateType : byte { Idle, Move, Attack };
[Sprite("character-prefabs", "gnome-mage")]
internal sealed class GnomeMage : Entity, IEnemy
{
    private bool _warding;
    private bool _attacking;

    private Vector2 _position;

    private sbyte _currentHealth;
    private readonly sbyte _maxHealth;
    private bool _isDead;

    private Texture2D _pixel;

    public GnomeMage(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _position = new Vector2(
                10 * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
                10 * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f
            );

        _maxHealth = 7;
        _currentHealth = _maxHealth;
    }

    private GnomeMage(GnomeMage gnomeMage)
    {
        _position = gnomeMage._position + new Vector2(10, 10);
    }

    public void Update(Player player, DungeonGeneration dungeon)
    {
        if (_isDead)
        {
            int deadTileX = (int)(_position.X / Tile.TILE_SIZE);
            int deadTileY = (int)(_position.Y / Tile.TILE_SIZE);

            if (deadTileX >= 0 && deadTileY >= 0 &&
                deadTileX < dungeon.Width && deadTileY < dungeon.Height)
            {
                Tile deadTile = dungeon.Tiles[deadTileX, deadTileY];
                if (deadTile.OccupiedBy == this)
                    deadTile.OccupiedBy = null;
            }
            return;
        }

        Vector2 playerPosition = player.Behavior.Position;

        int enemyTileX = (int)(_position.X / Tile.TILE_SIZE);
        int enemyTileY = (int)(_position.Y / Tile.TILE_SIZE);

        Tile currentTile = dungeon.Tiles[enemyTileX, enemyTileY];
        currentTile.OccupiedBy = this;

        int playerTileX = (int)(playerPosition.X / Tile.TILE_SIZE);
        int playerTileY = (int)(playerPosition.Y / Tile.TILE_SIZE);

        int dx = playerTileX - enemyTileX;
        int dy = playerTileY - enemyTileY;

        int distance = Math.Max(Math.Abs(dx), Math.Abs(dy));

        if (distance > 3)
        {
            Game.State = GameState.PlayerTurn;
            return;
        }

        _warding = true;

        if (distance <= 1)
        {
            _attacking = true;
            _warding = false;

            player.DB.TakeDamage(2);

            Game.State = GameState.PlayerTurn;
            return;
        }

        int moveX = Math.Sign(dx);
        int moveY = Math.Sign(dy);

        int newTileX = enemyTileX + moveX;
        int newTileY = enemyTileY + moveY;

        if (newTileX < 0 || newTileY < 0 ||
            newTileX >= dungeon.Width || newTileY >= dungeon.Height)
        {
            Game.State = GameState.PlayerTurn;
            return;
        }

        Tile targetTile = dungeon.Tiles[newTileX, newTileY];

        if (targetTile.OccupiedBy != null || !targetTile.IsWalkable)
        {
            Game.State = GameState.PlayerTurn;
            return;
        }

        currentTile.OccupiedBy = null;

        _position = new Vector2(
            newTileX * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
            newTileY * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f);

        targetTile.OccupiedBy = this;

        Game.State = GameState.PlayerTurn;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isDead)
        {
            base.Draw(spriteBatch, _position, Layer.EntityLayer);
            if (_attacking)
                DrawHealthBar(spriteBatch, Layer.GUILayer);
        }
    }

    private void DrawHealthBar(SpriteBatch spriteBatch, Layer layer)
    {
        int barWidth = Tile.TILE_SIZE - 2;
        int barHeight = 2;

        int healthBarX = (int)_position.X + (Tile.TILE_SIZE - barWidth) / 2;
        int healthBarY = (int)_position.Y - barHeight - 2;
        float healthPercentage = (float)_currentHealth / _maxHealth;
        int filledWidth = (int)(barWidth * healthPercentage);

        Vector2 origin = new Vector2(barWidth / 2, -Tile.TILE_SIZE / 2 + barHeight);

        spriteBatch.Draw(_pixel, _position, new Rectangle(healthBarX, healthBarY, barWidth, barHeight), Color.Black * 0.45f, 0f, origin, 1f, SpriteEffects.None, layer.Depth);
        spriteBatch.Draw(_pixel, _position, new Rectangle(healthBarX, healthBarY, filledWidth, barHeight), Color.Red, 0f, origin, 1f, SpriteEffects.None, layer.Depth + 0.01f);
    }

    public IEnemy Clone() => new GnomeMage(this);

    public void TakeDamage(int damage)
    {
        _currentHealth -= (sbyte)damage;
        if (checked(_currentHealth <= 0))
            _isDead = true;
    }
}