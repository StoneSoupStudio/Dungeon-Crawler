namespace DungeonCrawler;

public enum GameState : byte { PlayerTurn, EnemyTurn };
public sealed class Game : Core
{
    public const ushort SCREEN_WIDTH = 800;
    public const ushort SCREEN_HEIGHT = 608;

    public const byte BASE_CAMERA_OFFSET_X = 5 * Tile.TILE_SIZE;

    private DungeonGeneration dungeon;
    private Canvas canvas;

    private Camera2D _camera;

    private Player player;
    private GnomeMage gnomeMage;
    private GnomeMage gnomeMage2;

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
        dungeon = new(Content, 30, 30);

        player = new();
        gnomeMage = new();
        gnomeMage2 = (GnomeMage)gnomeMage.Clone();
    }

    private void LateInitialize()
    {
        player.Behavior.SpawnHeroInDungeon(dungeon, new(15, 10));
        _camera = new(GraphicsDevice.Viewport);

        canvas = new(GraphicsDevice, Content);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        player.Update(dungeon);

        gnomeMage.Update(player);
        gnomeMage2.Update(player);

        Vector2 target = new Vector2(BASE_CAMERA_OFFSET_X, 0) + player.Behavior.Position;
        _camera.Follow(target);

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

        player.Draw(SpriteBatch);
        gnomeMage.Draw(SpriteBatch);
        gnomeMage2.Draw(SpriteBatch);

        SpriteBatch.End();
    }

    private void DrawUI()
    {
        SpriteBatch.Begin(SpriteSortMode.Texture, samplerState: SamplerState.PointClamp);

        canvas.Draw(SpriteBatch, Layer.UILayer, player);

        SpriteBatch.End();
    }
}