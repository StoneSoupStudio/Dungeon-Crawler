using static Microsoft.Xna.Framework.Input.Keyboard;

namespace DungeonCrawler;

public sealed class Keyboard
{
    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    public Keyboard()
    {
        PreviousState = new();
        CurrentState = GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    public bool IsKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    public bool IsKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }
}