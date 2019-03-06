using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Keys", menuName = "Data/Keys")]
public class Keys: ScriptableObject
{
    public string Up;
    public string Down;
    public string Left;
    public string Right;
    public string Run;
    public string Reload;
}