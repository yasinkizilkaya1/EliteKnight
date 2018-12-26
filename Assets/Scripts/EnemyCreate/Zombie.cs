using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Zombie", menuName = "Data/Zombie")]
public class Zombie:DamageableEntity
{
    public int Speed;
    public int attackpower;
}