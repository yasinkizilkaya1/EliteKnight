using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : MoveableEntity
{
    public int Energy;
    public int MaxEnergy;
    public int EnergyIncreaseAmmount;
    public int Power;
    public int RunSpeed;
    public GameObject GameObject;

    public CharacterData(int id,string name,int health,int maxhealth,int speed,int defence,int energy,int maxenergy,int energyIncreaseAmmount,int power,int runspeed)
    {
        Id = id;
        Name = name;
        Health = health;
        MaxHealth = maxhealth;
        Speed = speed;
        Defence = defence;
        Energy = energy;
        MaxEnergy = maxhealth;
        EnergyIncreaseAmmount = energyIncreaseAmmount;
        Power = power;
        RunSpeed = runspeed;
    }
}