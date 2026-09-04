namespace DungeonCrawler;

internal enum StateType : byte { Idle, Walk, Attack, Dead }
internal abstract class Enemy(string name, RaceType race) : Entity
{
    protected readonly string _name = name;

    protected StateType _state;
    protected Vector2 _position;

    protected HealthBar _healthBar = new();
    protected bool _drawHealthBar;

    protected StatDB _DB = new(race);

    public void Update(Dungeon dungeon, ref Player player)
    {
        AI(dungeon, ref player);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!(_state == StateType.Dead))
        {
            base.Draw(spriteBatch, _position, Layer.EntityLayer);
            if (_drawHealthBar)
                _healthBar.Draw(spriteBatch, Layer.GUILayer, _position, _DB.CurrentHealth, _DB.MaxHealth, Color.Black, Color.Red);
        }
    }

    protected virtual void AI(Dungeon dungeon, ref Player player)
    {
        if (_state == StateType.Dead)
        {
            Dead(dungeon);
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

        if (distance >= 5)
        {
            _drawHealthBar = false;
        }
        else if (distance > 2)
        {
            _state = StateType.Idle;
            Game.State = GameState.PlayerTurn;
            return;
        }

        if (_DB.CurrentHealth != _DB.MaxHealth && _state == StateType.Idle)
            _drawHealthBar = true;

        _state = StateType.Walk;

        if (distance <= 1)
        {
            _state = StateType.Attack;
            _drawHealthBar = true;

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

    public void TakeDamage(ushort damage)
    {
        _DB.CurrentHealth -= damage;
        if (checked(_DB.CurrentHealth <= 0))
            _state = StateType.Dead;
    }

    private void Dead(Dungeon dungeon)
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

    public void Spawn(Dungeon dungeon, Point cell)
    {
        if (cell.X >= 0 && cell.X < dungeon.Width &&
            cell.Y >= 0 && cell.Y < dungeon.Height)
        {
            _position = new Vector2(
                cell.X * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
                cell.Y * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f
            );
        }
        else
        {
            cell = new Point(
                dungeon.Width / 2,
                dungeon.Height / 2
            );
            _position = new Vector2(
                cell.X * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f,
                cell.Y * Tile.TILE_SIZE + Tile.TILE_SIZE / 2f
            );
        }
        dungeon.Tiles[cell.X, cell.Y].OccupiedBy = this;
    }
}