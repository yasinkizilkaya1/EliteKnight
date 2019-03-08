using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New weapon", menuName = "Data/Weapon")]
public class Weapon : Item
{
    public int Power;
    public int ClipCapacity;
    public int TotalBullet;
    public float Range;
    public float ReloadTime;
    public GameObject WeaponObject;

    public override void Use(Character character)
    {
        character.GunChange(WeaponObject);
    }
}