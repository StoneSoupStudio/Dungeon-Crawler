namespace DungeonCrawler;

internal sealed class Camera2D(Viewport viewport)
{
    private readonly Viewport _viewport = viewport;

    private Vector2 _position = Vector2.Zero;
    public Vector2 Position => _position;

    public readonly float Zoom = 1f;
    public float Rotation { get; } = 0f;

    public Matrix Transform =>
        Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
        Matrix.CreateRotationZ(Rotation)*
        Matrix.CreateScale(Zoom, Zoom, 1f) *
        Matrix.CreateTranslation(new Vector3(_viewport.Width* 0.5f, _viewport.Height* 0.5f, 0f));

    public void Follow(Vector2 target)
    {
        _position = target;
    }
}