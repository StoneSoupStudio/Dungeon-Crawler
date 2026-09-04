namespace DungeonCrawler;

internal sealed class PlayerBehavior
{
    private Vector2 _position;
    public Vector2 Position => _position;

    private const float MoveCooldown = 0.15f;
    private float _moveTimer;

    private static readonly (Keys[] Keys, Point Direction)[] _moveBindings =
    {
        (new[] { Keys.W, Keys.Up,    Keys.NumPad8 }, new Point( 0, -1)),
        (new[] { Keys.S, Keys.Down,  Keys.NumPad2 }, new Point( 0,  1)),
        (new[] { Keys.A, Keys.Left,  Keys.NumPad4 }, new Point(-1,  0)),
        (new[] { Keys.D, Keys.Right, Keys.NumPad6 }, new Point( 1,  0)),
        (new[] { Keys.NumPad7 },                     new Point(-1, -1)),
        (new[] { Keys.NumPad9 },                     new Point( 1, -1)),
        (new[] { Keys.NumPad1 },                     new Point(-1,  1)),
        (new[] { Keys.NumPad3 },                     new Point( 1,  1)),
    };

    private void HandlerInput(ref Point moveDelta, GameTime gameTime)
    {
        moveDelta = Point.Zero;

        _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_moveTimer < MoveCooldown)
            return;

        Point direction = Point.Zero;

        foreach (var (keys, dir) in _moveBindings)
        {
            if (Input.Keyboard.IsAnyKeyDown(keys))
            {
                direction = dir;
                break;
            }
        }

        if (direction == Point.Zero)
            return;

        moveDelta = new Point(
            direction.X * Tile.TILE_SIZE,
            direction.Y * Tile.TILE_SIZE);

        _moveTimer = 0f;
    }

    public void Move(GameTime gameTime, Dungeon dungeon)
    {
        Point oldPosition = _position.ToPoint();

        Point moveDelta = Point.Zero;
        HandlerInput(ref moveDelta, gameTime);

        if (moveDelta == Point.Zero)
            return;

        Point newPosition = oldPosition + moveDelta;

        int tileX = newPosition.X / Tile.TILE_SIZE;
        int tileY = newPosition.Y / Tile.TILE_SIZE;

        if (tileX < 0 || tileY < 0 ||
            tileX >= dungeon.Width || tileY >= dungeon.Height)
        {
            return;
        }

        Tile targetTile = dungeon.Tiles[tileX, tileY];

        if (targetTile.OccupiedBy is GnomeMage enemy)
        {
            enemy.TakeDamage(5);
            Game.State = GameState.EnemyTurn;
            return;
        }

        if (targetTile.OccupiedBy != null)
            return;

        if (!targetTile.IsWalkable)
            return;

        int oldTileX = oldPosition.X / Tile.TILE_SIZE;
        int oldTileY = oldPosition.Y / Tile.TILE_SIZE;

        dungeon.Tiles[oldTileX, oldTileY].OccupiedBy = null;

        _position = newPosition.ToVector2();

        dungeon.Tiles[tileX, tileY].OccupiedBy = this;

        Game.State = GameState.EnemyTurn;
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