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
    inventory,
    exit,
    slot
}

[System.Serializable]
public class Key
{
    public KeyType KeyType;
    public KeyCode CurrentKey;
    public KeyCode DefaultKey;
}