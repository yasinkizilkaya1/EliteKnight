using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region Fields

    public Item Item;
    public bool IsFull;
    public bool IsUse;
    public Image Image;

    public GameManager GameManager;

    #endregion 

    public void OnSetItemUseButtonClicked()
    {
        if (IsUse)
        {
            GameManager.Inventory.ItemUse(this);
        }
    }
}