using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    up,
    down,
    right,
    left,
    run,
    reload,
    shoot,
    inventory
}

[System.Serializable]
public class Key
{
    public KeyType KeyType;
    public KeyCode CurrentKey;
    public KeyCode DefaultKey;
}

[CreateAssetMenuAttribute(fileName = "New Keys", menuName = "Data/KeySettings")]
public class KeySettings : ScriptableObject
{
    public List<Key> Keys;

    public void KeysReset()
    {
        foreach (Key item in Keys)
        {
            item.CurrentKey = item.DefaultKey;
        }
    }
}
// linq