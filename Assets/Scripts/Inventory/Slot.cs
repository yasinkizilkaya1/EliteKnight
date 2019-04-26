using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region Fields

    public Item Item;
    public bool IsFull;
    public Image Image;

    public GameManager gameManager;

    #endregion 

    public void OnSetItemUseButtonClicked()
    {
        gameManager.Inventory.ItemUse(this);
    }
}