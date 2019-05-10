using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New KeySettings", menuName = "Data/KeySettings")]
public class KeySetting : ScriptableObject
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