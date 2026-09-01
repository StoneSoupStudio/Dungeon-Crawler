namespace DungeonCrawler;

internal sealed class PlayerBehavior
{
    private Vector2 _position;
    public Vector2 Position => _position;

    public void HandlerInput()
    {
        if (Input.Keyboard.IsKeyJustPressed(Keys.W) || Input.Keyboard.IsKeyJustPressed(Keys.Up) || Input.Keyboard.IsKeyJustPressed(Keys.NumPad8))
            _position.Y -= 1 * Tile.TILE_SIZE;
        if (Input.Keyboard.IsKeyJustPressed(Keys.S) || Input.Keyboard.IsKeyJustPressed(Keys.Down) || Input.Keyboard.IsKeyJustPressed(Keys.NumPad2))
            _position.Y += 1 * Tile.TILE_SIZE;
        if (Input.Keyboard.IsKeyJustPressed(Keys.A) || Input.Keyboard.IsKeyJustPressed(Keys.Left) || Input.Keyboard.IsKeyJustPressed(Keys.NumPad4))
            _position.X -= 1 * Tile.TILE_SIZE;
        if (Input.Keyboard.IsKeyJustPressed(Keys.D) || Input.Keyboard.IsKeyJustPressed(Keys.Right) || Input.Keyboard.IsKeyJustPressed(Keys.NumPad6))
            _position.X += 1 * Tile.TILE_SIZE;

        if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad9))
            _position += new Vector2(1 * Tile.TILE_SIZE, -1 * Tile.TILE_SIZE);
        if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad7))
            _position -= new Vector2(1 * Tile.TILE_SIZE, 1 * Tile.TILE_SIZE);
        if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad3))
            _position += new Vector2(1 * Tile.TILE_SIZE, 1 * Tile.TILE_SIZE);
        if (Input.Keyboard.IsKeyJustPressed(Keys.NumPad1))
            _position -= new Vector2(1 * Tile.TILE_SIZE, -1 * Tile.TILE_SIZE);
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