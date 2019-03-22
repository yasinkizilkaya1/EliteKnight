using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private const string TAG_STAGE = "Stage";

    public CardContent CardContent;
    public CharacterData CharacterData;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnPlayButtonClicked()
    {
        for (int i = 0; i < CardContent.ToggleList.Count; i++)
        {
            if(CardContent.ToggleList[i].isOn)
            {
                CharacterData = CardContent.CharacterList[i];
                SceneManager.LoadScene(TAG_STAGE);
            }
        }
    }
}