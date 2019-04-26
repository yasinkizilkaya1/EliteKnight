using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Tower", menuName = "Data/Tower")]
public class Tower:FencibleEntity
{
    public int AttacPower;
    public float AttackTime;
}