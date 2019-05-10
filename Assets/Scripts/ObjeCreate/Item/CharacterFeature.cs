using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New CharacterFeatureItem", menuName = "Data/CharacterFeatureItem")]
public class CharacterFeature : Item
{
    public int Health;
    public int Defence;
    public int Speed;
    public int Energy;
    public int EnergyIncreaseAmmaunt;
    public int Ammo;

    public override void Use(Character character)
    {
        character.CurrentHP += Health;
        character.CurrentDefence += Defence;
        character.Speed += Speed;
        character.Energy += Energy;
        character.Gun.SpareBulletCount += Ammo;
        character.UIManager.AmmoBar.ClipAmountText.text = character.Gun.SpareBulletCount.ToString();
    }
}