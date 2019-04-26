using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New bullet", menuName = "Data/bullet")]
public class bullet :Entity
{
    public bool IsFollow;
    public bool IsExplode;
    public bool IsEnemyCreate;

    public int Power;
    public int Speed;
}