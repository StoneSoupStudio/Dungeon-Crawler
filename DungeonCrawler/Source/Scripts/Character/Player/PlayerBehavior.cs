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
        Point movePoint = _position.ToPoint();

        HandlerInput(ref movePoint);

        if (movePoint == _position.ToPoint())
            return;

        if (!CanMoveTo(dungeon, movePoint))
            return;

        _position = movePoint.ToVector2();
    }

    private bool CanMoveTo(DungeonGeneration dungeon, Point position)
    {
        int tileX = position.X / Tile.TILE_SIZE;
        int tileY = position.Y / Tile.TILE_SIZE;

        if (tileX < 0 || tileY < 0 || tileX >= dungeon.Width || tileY >= dungeon.Height)
        {
            return false;
        }

        return dungeon.Tiles[tileX, tileY].IsWalkable;
    }


    public void SpawnHeroInDungeon(DungeonGeneration dungeon, Point cell)
    {
        if (cell.X >= 0 && cell.X < dungeon.Width &&
            cell.Y >= 0 && cell.Y < dungeon.Height)
        {
            _position = new Vector2(cell.X * Tile.TILE_SIZE, cell.Y * Tile.TILE_SIZE);
        }
        else
        {
            _position = new Vector2(dungeon.Width / 2 * Tile.TILE_SIZE, dungeon.Height / 2 * Tile.TILE_SIZE);
        }
    }
}