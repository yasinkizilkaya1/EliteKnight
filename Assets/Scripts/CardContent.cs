using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardContent : MonoBehaviour
{
    #region Fields

    public GameObject cardPrefabObject;

    public List<CharacterData> CharacterList;
    public List<Toggle> ToggleList;

    public ToggleGroup ToggleGroup;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    #endregion

    #region Private Method

    public void Initialize()
    {
        ToggleList = new List<Toggle>();

        for (int i = 0; i < CharacterList.Count; i++)
        {
            GameObject card = Instantiate(cardPrefabObject, transform);
            card.GetComponent<Card>().UpdateData(CharacterList[i]);
            Toggle toggle = card.GetComponent<Toggle>();
            toggle.group = ToggleGroup;
            ToggleList.Add(toggle);
        }
    }

    #endregion
}