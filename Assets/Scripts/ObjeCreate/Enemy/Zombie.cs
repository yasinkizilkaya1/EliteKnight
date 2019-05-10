using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Zombie", menuName = "Data/Zombie")]
public class Zombie : DetriMentalEntity
{
    public float AttackRange;
    public float ShootingRate;
    public float RotationSpeed;
}