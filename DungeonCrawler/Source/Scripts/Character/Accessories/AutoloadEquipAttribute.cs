namespace DungeonCrawler;

public enum EquipType : byte { None = 0, Feet, Legs, Body, Belt, Hands, Head, Back };
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal sealed class AutoloadEquipAttribute : Attribute
{
    public EquipType Type { get; }

    public AutoloadEquipAttribute(EquipType type)
    {
        Type = type;
    }
}