using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Chest Entity", menuName = "Data/Chest")]
public class ChestEntity : DamageableEntity
{
    public bool ItemDrop;
}