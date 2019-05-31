using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public GameObject ItemObject;
    public int StackSize;
    public int MaxStackSize;

    public abstract void Use(Character character);
}