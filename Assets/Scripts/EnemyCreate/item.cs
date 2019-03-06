using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Item", menuName = "Data/Item")]

public class item : Entity
{
    public int Clip;
    public int Health;
    public int Defence;
    public Type type;

    public enum Type
    {
        Perish,
        Weapon
    }
}