namespace DungeonCrawler;

public enum GameState : byte { PlayerTurn, EnemyTurn };
public sealed class Game : Core
{
    public const ushort SCREEN_WIDTH = 800;
    public const ushort SCREEN_HEIGHT = 608;

    public const byte BASE_CAMERA_OFFSET_X = 5 * Tile.TILE_SIZE;

    private Dungeon dungeon;
    private Canvas canvas;

    private Camera2D _camera;

    private Player player;

    private Enemy gnomeMage;
    private Enemy gnomeBarbarian;
    private Enemy gnomePriest;
    private Enemy gnomeHoly;

    private List<Enemy> _enemies;

    private Minimap minimap;
    private FogOfWar fogOfWar;

    public static GameState State { get; set; }

    public Game() : base("Dungeon Crawler", SCREEN_WIDTH, SCREEN_HEIGHT, false)
    {

    }

    protected override void Initialize()
    {
        PreInitialize();

        base.Initialize();

        LateInitialize();
    }

    private void PreInitialize()
    {
        dungeon = new(Content, 50, 50);
    }

    private void LateInitialize()
    {
        player = new(GraphicsDevice, Content);

        gnomeMage = new GnomeMage();
        gnomeBarbarian = new GnomeBarbarian();
        gnomePriest = new GnomePriest();
        gnomeHoly = new GnomeHoly();

        _enemies = new List<Enemy> { gnomeMage, gnomeBarbarian, gnomePriest, gnomeHoly };

        fogOfWar = new(GraphicsDevice, dungeon);

        player.Behavior.Spawn(dungeon, new(25, 25));

        gnomeMage.Spawn(dungeon, new(10, 10));
        gnomeBarbarian.Spawn(dungeon, new(15, 12));
        gnomePriest.Spawn(dungeon, new(12, 10));
        gnomeHoly.Spawn(dungeon, new(11, 12));

        _camera = new(GraphicsDevice.Viewport);
        minimap = new(GraphicsDevice, dungeon);

        canvas = new(GraphicsDevice, Content);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        player.Update(dungeon, gameTime);
        minimap.Update(SpriteBatch);

        switch (State)
        {
            case GameState.EnemyTurn:

                //foreach (Enemy enemy in _enemies)
                   // enemy.Update(dungeon, ref player);

                State = GameState.PlayerTurn;
                break;

            default:
                break;
        }

        Vector2 target = new Vector2(BASE_CAMERA_OFFSET_X, 0) + player.Behavior.Position;
        _camera.Follow(target);

        fogOfWar.Update(player.Behavior.Position);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        DrawGame();
        DrawUI();
    }

    private void DrawGame()
    {
        SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);

        dungeon.Draw(SpriteBatch);
        fogOfWar.DrawFog(SpriteBatch, Layer.GUILayer);
        minimap.MarkDirty();

        player.Draw(SpriteBatch);

        foreach (Enemy enemy in _enemies)
            enemy.Draw(SpriteBatch);

        SpriteBatch.End();
    }

    private void DrawUI()
    {
        SpriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

        canvas.Draw(SpriteBatch, Layer.UILayer, player);
        minimap.Draw(SpriteBatch, player.Behavior.Position);

        SpriteBatch.End();
    }
}