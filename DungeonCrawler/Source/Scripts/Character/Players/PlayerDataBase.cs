namespace DungeonCrawler;

internal sealed class PlayerDB
{
    public string Name { get; }
    public RaceType Race { get; }

    public ushort CurrentHealth { get; }
    public ushort MaxHealth { get; private set; }

    public byte CurrentMana { get; }
    public byte MaxMana { get; private set; }

    public byte Strength { get; }
    public byte Dexterity { get; }
    public byte Constitution { get; }
    public byte Intelligence { get; }
    public byte Wisdom { get; }
    public byte Charisma { get; }
    public sbyte Luck { get; }

    public PlayerDB()
    {
        Name = "ZZZ";
        Race = RaceType.Human;

        FinallyCharacteristic();
    }

    private void FirstCharacteristic()
    {
        switch (Race)
        {
            case RaceType.Human:

                break;
            case RaceType.Demon:

                break;
            default:
                break;
        }
    }

    private void CalculateCharacteristic()
    {
        Random rnd = new Random();

        byte add = (byte)rnd.Next(0, 20 + 1);

        sbyte dopStrength;

        if (add == 20)
        {
            dopStrength = 3;
        }
        else if (add >= 15)
        {
            dopStrength = 2;
        }
        else if (add >= 12)
        {
            dopStrength = 1;
        }
        else if (add >= 10)
        {
            dopStrength = 0;
        }
        else if (add >= 8)
        {
            dopStrength = -1;
        }
        else
        {
            dopStrength = -2;
        }
    }

    private void FinallyCharacteristic()
    {

    }
}