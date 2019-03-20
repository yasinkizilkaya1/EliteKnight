public class CharacterFeature : Item
{
    public int Health;
    public int Defence;
    public int Speed;
    public int Energy;
    public int EnergyIncreaseAmmaunt;

    public override void Use(Character character)
    {
        character.CurrentHP += Health;
        character.CurrentDefence += Defence;
        character.Speed += Speed;
        character.Energy += Energy;
    }
}