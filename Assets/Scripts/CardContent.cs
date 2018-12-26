using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardContent : MonoBehaviour
{
    #region Fields

    public GameObject cardPrefabObject;

    public List<CharacterData> CharacterList;
    public List<Toggle> ToggleList;

    public bool isToggle1;
    public bool isToggle2;
    public bool isToggle3;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        isToggle1 = ToggleList[0].isOn;
        isToggle2 = ToggleList[1].isOn;
        isToggle3 = ToggleList[2].isOn;
    }

    #endregion

    #region Private Method

    public void Initialize()
    {
        ToggleList = new List<Toggle>();

        for (int i = 0; i < CharacterList.Count; i++)
        {
            GameObject card = Instantiate(cardPrefabObject, transform);
            card.GetComponent<Card>().UpEnumdateData(CharacterList[i]);
            ToggleList.Add(card.GetComponent<Toggle>());
        }
    }

    #endregion
}