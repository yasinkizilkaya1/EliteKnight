using UnityEngine;

[System.Serializable]
public class CharacterData : ScriptableObject
{
    public int Id;
    public string Name;
    public int Health;
    public int MaxHealth;
    public int Energy;
    public int MaxEnergy;
    public int Power;
    public int Defence;
    public int Speed;
}