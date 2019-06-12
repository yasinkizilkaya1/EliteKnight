using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    #region Fields

    public Item Item;
    public bool IsFull;
    public bool IsUse;
    public Image Image;

    public Inventory Inventory;

    public Toggle Toggle;
    public List<Button> Buttons;

    private bool IsClick;

    #endregion

    #region Unity Method

    private void Update()
    {
        if (Toggle.isOn == false && IsClick == true)
        {
            foreach (Button button in Buttons)
            {
                button.gameObject.SetActive(false);
            }
            IsClick = false;
        }
    }

    #endregion

    #region Events

    public void OnSetSlotButtonClicked()
    {
        IsClick = true;

        if (IsUse)
        {
            foreach (Button button in Buttons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            if (Inventory.GameManager.Character.Guns.Count > 1)
            {
                Buttons[1].gameObject.SetActive(true);
            }
        }
    }

    public void OnSetItemDropButtonClicked()
    {
        if (IsUse)
        {
            Inventory.ItemDrop(this, Item.ItemObject);
        }
        else
        {
            Inventory.GameManager.GunSlot.GunDrop(Inventory.GameManager.Character, Item, this);
        }
    }

    public void OnSetItemUseButtonClicked()
    {
        Inventory.ItemUse(this);
    }

    #endregion
}