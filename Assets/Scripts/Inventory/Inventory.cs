using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Fields

    public List<Slot> Slots;
    public Slot Slot;
    public GameObject Content;

    public GameManager GameManager;

    #endregion

    #region Private Methods

    private void ItemEqual(Slot mSlot, Item item,bool isUse)
    {
        mSlot.Item = item;
        mSlot.Image.sprite = item.Icon;
        mSlot.IsFull = true;
        mSlot.IsUse = isUse;
    }

    #endregion

    #region Public Methods

    public void ItemAdd(Item item,bool isUse)
    {
        if (Slots.Count == 0)
        {
            Slot mSlot = Instantiate(Slot, Content.transform);
            mSlot.GameManager = GameManager;
            Slots.Add(mSlot);
            ItemEqual(mSlot, item,isUse);
            return;
        }
        else
        {
            foreach (Slot slot in Slots)
            {
                if (slot.IsFull == false)
                {
                    ItemEqual(slot, item,isUse);
                    return;
                }
                else
                {
                    Slot mSlot = Instantiate(Slot, Content.transform);
                    mSlot.GameManager = GameManager;
                    Slots.Add(mSlot);
                    ItemEqual(mSlot, item,isUse);
                    return;
                }
            }
        }
    }

    public void ItemUse(Slot slot)
    {
        slot.Item.Use(GameManager.Character);
        Slots.Remove(slot);
        Destroy(slot.gameObject);
    }

    #endregion
}