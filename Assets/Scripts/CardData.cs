public class CardData : MovableEntity
{
    public int Power;

    public CardData(int id, string name, int health, int maxHealth, int energy, int maxEnergy, int power, int speed, int defence)
    {
        Id = id;
        Name = name;
        Health = health;
        MaxHealth = maxHealth;
        Energy = energy;
        MaxEnergy = maxEnergy;
        Power = power;
        Speed = speed;
        Defence = defence;
    }
}