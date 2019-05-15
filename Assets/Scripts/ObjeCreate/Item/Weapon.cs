using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New weapon", menuName = "Data/Weapon")]
public class Weapon : Item
{
    public int Power;
    public int ClipCapacity;
    public int TotalBullet;
    public float Range;
    public float ReloadTime;
    public bool IsAutoReload;
    public bool IsAttak;
    public Sprite Bullet;

    public override void Use(Character character)
    {
        IsAttak = true;
    }
}