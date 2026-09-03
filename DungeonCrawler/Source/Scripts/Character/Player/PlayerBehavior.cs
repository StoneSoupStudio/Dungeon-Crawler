namespace DungeonCrawler;

internal sealed class PlayerBehavior
{
    private Vector2 _position;
    public Vector2 Position => _position;

    private void HandlerInput(ref Point movePoint)
    {
        if (Input.Keyboard.IsKeyJustPressed(Keys.W) ||
            Input.Keyboard.IsKeyJustPressed(Keys.Up) ||
            Input.Keyboard.IsKeyJustPressed(Keys.NumPad8))
        {
            movePoint.Y -= Tile.TILE_SIZE;
        }
        else if (Input.Keyboard.IsKeyJustPressed(Keys.S) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.Down) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.NumPad2))
        {
            movePoint.Y += Tile.TILE_SIZE;
        }
        else if (Input.Keyboard.IsKeyJustPressed(Keys.A) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.Left) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.NumPad4))
        {
            movePoint.X -= Tile.TILE_SIZE;
        }
        else if (Input.Keyboard.IsKeyJustPressed(Keys.D) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.Right) ||
                 Input.Keyboard.IsKeyJustPressed(Keys.NumPad6))
        {
            movePoint.X += Tile.TILE_SIZE;
        }

        if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad1))
            movePoint += new Point(-Tile.TILE_SIZE, Tile.TILE_SIZE);
        else if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad3))
            movePoint += new Point(Tile.TILE_SIZE, Tile.TILE_SIZE);
        else if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad7))
            movePoint += new Point(-Tile.TILE_SIZE, -Tile.TILE_SIZE);
        else if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad9))
            movePoint += new Point(Tile.TILE_SIZE, -Tile.TILE_SIZE);
    }

    public void Move(DungeonGeneration dungeon)
    {
        Point oldPosition = _position.ToPoint();
        Point movePoint = oldPosition;

        HandlerInput(ref movePoint);

        if (movePoint == oldPosition)
            return;

        int tileX = movePoint.X / Tile.TILE_SIZE;
        int tileY = movePoint.Y / Tile.TILE_SIZE;

        // Проверяем границы
        if (tileX < 0 || tileY < 0 ||
            tileX >= dungeon.Width || tileY >= dungeon.Height)
        {
            return;
        }

        Tile targetTile = dungeon.Tiles[tileX, tileY];

        // ==========================================
        // АТАКА
        // ==========================================

        if (targetTile.OccupiedBy is GnomeMage enemy)
        {
            enemy.TakeDamage(5);

            Game.State = GameState.EnemyTurn;
            return;
        }

        // ==========================================
        // КЛЕТКА ЗАНЯТА КЕМ-ТО ДРУГИМ
        // ==========================================

        if (targetTile.OccupiedBy != null)
            return;

        // ==========================================
        // ПРОВЕРКА ПРОХОДИМОСТИ
        // ==========================================

        if (!targetTile.IsWalkable)
            return;

        // ==========================================
        // ОСВОБОЖДАЕМ СТАРУЮ КЛЕТКУ
        // ==========================================

        int oldTileX = oldPosition.X / Tile.TILE_SIZE;
        int oldTileY = oldPosition.Y / Tile.TILE_SIZE;

        dungeon.Tiles[oldTileX, oldTileY].OccupiedBy = null;

        // ==========================================
        // ПЕРЕМЕЩАЕМ ИГРОКА
        // ==========================================

        _position = movePoint.ToVector2();

        // ==========================================
        // ЗАНИМАЕМ НОВУЮ КЛЕТКУ
        // ==========================================

        dungeon.Tiles[tileX, tileY].OccupiedBy = this;

        Game.State = GameState.EnemyTurn;
    }

    public void SpawnHeroInDungeon(DungeonGeneration dungeon, Point cell)
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