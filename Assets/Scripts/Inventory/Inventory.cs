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

    private void ItemEqual(Slot mSlot, Item item)
    {
        mSlot.Item = item;
        mSlot.Image.sprite = item.Icon;
        mSlot.IsFull = true;
    }

    #endregion

    #region Public Methods

    public void ItemAdd(Item item)
    {
        if (Slots.Count == 0)
        {
            Slot mSlot = Instantiate(Slot, Content.transform);
            mSlot.gameManager = GameManager;
            Slots.Add(mSlot);
            ItemEqual(mSlot, item);
            return;
        }
        else
        {
            foreach (Slot slot in Slots)
            {
                if (slot.IsFull == false)
                {
                    ItemEqual(slot, item);
                    return;
                }
                else
                {
                    Slot mSlot = Instantiate(Slot, Content.transform);
                    mSlot.gameManager = GameManager;
                    Slots.Add(mSlot);
                    ItemEqual(mSlot, item);
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