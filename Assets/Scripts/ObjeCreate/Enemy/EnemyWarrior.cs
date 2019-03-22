using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Enemy Warrior", menuName = "Data/EnemyWarrior")]
public class EnemyWarrior : DetriMentalEntity
{
    public float AttackTime;
    public int Range;
    public int Distance;
}