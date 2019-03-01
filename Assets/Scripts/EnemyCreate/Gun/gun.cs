using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New gun", menuName = "Data/Gun")]
public class gun : Entity
{
    public int Power;
    public float Range;
    public int ClipCapacity;
    public int TotalBullet;
    public float ReloadTime;
}