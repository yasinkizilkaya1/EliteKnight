using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private const string TAG_STAGE = "Stage";

    public CardContent CardContent;
    public CharacterData CharacterData;

    public void OnPlayButtonClicked()
    {
        for (int i = 0; i < CardContent.ToggleList.Count; i++)
        {
            if (CardContent.ToggleList[i].isOn)
            {
                CharacterData.Id = CardContent.CharacterList[i].Id;
                CharacterData.Name = CardContent.CharacterList[i].Name;
                CharacterData.Health = CardContent.CharacterList[i].Health;
                CharacterData.MaxHealth = CardContent.CharacterList[i].MaxHealth;
                CharacterData.Defence = CardContent.CharacterList[i].Defence;
                CharacterData.Energy = CardContent.CharacterList[i].Energy;
                CharacterData.MaxEnergy = CardContent.CharacterList[i].MaxEnergy;
                CharacterData.Power = CardContent.CharacterList[i].Power;
                CharacterData.Speed = CardContent.CharacterList[i].Speed;
                CharacterData.RunSpeed = CardContent.CharacterList[i].RunSpeed;
                CharacterData.EnergyReloadTime = CardContent.CharacterList[i].EnergyReloadTime;
                CharacterData.EnergyIncreaseAmmount = CardContent.CharacterList[i].EnergyIncreaseAmmount;
                CharacterData.GameObject = CardContent.CharacterList[i].GameObject;
                SceneManager.LoadScene(TAG_STAGE);
            }
        }
    }
}