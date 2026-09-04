namespace DungeonCrawler;

public class Core : Microsoft.Xna.Framework.Game
{
    internal static Core s_instance;

    public static Core Instance => s_instance;

    public static GraphicsDeviceManager Graphics { get; private set; }

    public static new GraphicsDevice GraphicsDevice { get; private set; }

    public static SpriteBatch SpriteBatch { get; private set; }

    public static new ContentManager Content { get; private set; }

    public static ImGuiRenderer ImGuiRenderer { get; private set; }

    private const byte FPS = 45;

    public Core(string title, int width, int height, bool fullScreen)
    {
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        s_instance = this;

        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = fullScreen
        };

        Graphics.ApplyChanges();

        Window.Title = title;

        Content = base.Content;

        Content.RootDirectory = "Source/Resources";

        TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / FPS);

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;

        ImGuiRenderer = new ImGuiRenderer(this);
        ImGuiRenderer.RebuildFontAtlas();

        var io = ImGui.GetIO();
        io.FontGlobalScale = 1.125f;
        ImGui.GetStyle().ScaleAllSizes(1.05f);

        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }
}