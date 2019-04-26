using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Boss", menuName = "Data/Boss")]
public class Boss:DamageableEntity
{
    public float AttackTime;
    public float shootcoolDown;
}