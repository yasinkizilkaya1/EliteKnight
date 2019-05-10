using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Boss", menuName = "Data/Boss")]
public class Boss : DamageableEntity
{
    public float WaitTime;
    public float AttackTime;
    public List<float> ShootcoolDowns;
    public float ShootAngle;
}