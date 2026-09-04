namespace DungeonCrawler;

internal sealed class Inventory
{
    private const byte INVENTORY_CAPACITY = 10;
    private Slot[] _slots;

    public Inventory(ContentManager content)
    {
        _slots = new Slot[INVENTORY_CAPACITY];
        for (int index = 0; index < _slots.Length; index++)
            _slots[index] = new(content);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Slot slot in _slots)
            slot.Draw(spriteBatch, Layer.UILayer);
    }
}