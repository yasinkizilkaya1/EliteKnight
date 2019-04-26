using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardContent : MonoBehaviour
{
    #region Fields

    public GameObject cardPrefabObject;

    public List<CharacterData> Characters;
    public List<Toggle> Toggles;

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
        Toggles = new List<Toggle>();

        for (int i = 0; i < Characters.Count; i++)
        {
            GameObject card = Instantiate(cardPrefabObject, transform);
            card.GetComponent<Card>().UpdateData(Characters[i]);
            Toggle toggle = card.GetComponent<Toggle>();
            toggle.group = ToggleGroup;
            Toggles.Add(toggle);
        }
    }

    #endregion
}