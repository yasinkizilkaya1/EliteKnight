using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Enemy Warrior", menuName = "Data/EnemyWarrior")]
public class EnemyWarrior:DamageableEntity
{
    public int Speed;
    public int AttackPower;
    public int Range;
    public int Distance;
}