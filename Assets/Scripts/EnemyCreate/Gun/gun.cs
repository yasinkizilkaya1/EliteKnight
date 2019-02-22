using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New gun", menuName = "Data/Gun")]
public class gun : Entity
{
    public int Power;
    public int ClipCapacity;
    public int TotalBullet;
    public float ReloadTime;
}