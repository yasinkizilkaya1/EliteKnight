using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterID : MonoBehaviour
{
    #region Constants

    private const string TAG_STAGE = "Stage";

    #endregion

    #region Fields

    public CardContent cardContent;

    public string SelectedCardNameString;

    #endregion

    #region Unity Methods

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (cardContent == null)
        {
            Destroy(gameObject, 6f);
        }
        else
        {
            if (cardContent.isToggle1)
            {
                SelectedCardNameString = cardContent.CharacterList[0].name;
            }
            else if (cardContent.isToggle2)
            {
                SelectedCardNameString = cardContent.CharacterList[1].name;
            }
            else if (cardContent.isToggle3)
            {
                SelectedCardNameString = cardContent.CharacterList[2].name;
            }
            else
            {
                SelectedCardNameString = cardContent.CharacterList[0].name;
            }
        }
    }

    #endregion

    #region Private Method

    public void GameStart()
    {
        SceneManager.LoadScene(TAG_STAGE);
    }

    #endregion
}