using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private const string mTAG_STAGE = "Stage";

    public CardContent CardContent;
    public CharacterData CharacterData;

    public void OnPlayButtonClicked()
    {
        for (int i = 0; i < CardContent.Toggles.Count; i++)
        {
            if (CardContent.Toggles[i].isOn)
            {
                CharacterData.Id = CardContent.Characters[i].Id;
                CharacterData.Name = CardContent.Characters[i].Name;
                CharacterData.Health = CardContent.Characters[i].Health;
                CharacterData.MaxHealth = CardContent.Characters[i].MaxHealth;
                CharacterData.Defence = CardContent.Characters[i].Defence;
                CharacterData.Energy = CardContent.Characters[i].Energy;
                CharacterData.MaxEnergy = CardContent.Characters[i].MaxEnergy;
                CharacterData.Power = CardContent.Characters[i].Power;
                CharacterData.Speed = CardContent.Characters[i].Speed;
                CharacterData.RunSpeed = CardContent.Characters[i].RunSpeed;
                CharacterData.EnergyIncreaseAmmount = CardContent.Characters[i].EnergyIncreaseAmmount;
                CharacterData.GameObject = CardContent.Characters[i].GameObject;
                SceneManager.LoadScene(mTAG_STAGE);
            }
        }
    }
}