using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New weapon", menuName = "Data/Weapon")]
public class Weapon : Item
{
    public bool IsAttak;
    public int Power;
    public int ClipCapacity;
    public int TotalBullet;
    public float Range;
    public float ReloadTime;

    public override void Use(Character character)
    {
        IsAttak = true;
    }
}