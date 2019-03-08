using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public int StackSize;

    public enum Type
    {
        Weapon,
        Bullet,
        Health,
        Defence,
        Clip
    }

    public abstract void Use(Character character);
}