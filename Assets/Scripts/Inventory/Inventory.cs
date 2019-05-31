using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Fields

    public List<Slot> Slots;
    public Slot Slot;
    public GameObject Content;

    public GameManager GameManager;

    #endregion

    #region Private Methods

    private void ItemEqual(Slot mSlot, Item item, bool isUse)
    {
        mSlot.Item = item;
        mSlot.Image.sprite = item.Icon;
        mSlot.IsFull = true;
        mSlot.IsUse = isUse;
    }

    private void CloseIsOnToggles()
    {
        foreach (Slot slot in Slots)
        {
            slot.Toggle.isOn = false;
        }
    }

    private void SlotStuff(Item item, bool isUse, Slot slot)
    {
        slot.Inventory = this;
        slot.Toggle.group = Content.GetComponent<ToggleGroup>();
        CloseIsOnToggles();
        Slots.Add(slot);
        ItemEqual(slot, item, isUse);
    }

    #endregion

    #region Public Methods

    public void ItemAdd(Item item, bool isUse)
    {
        if (Slots.Count == 0)
        {
            Slot mSlot = Instantiate(Slot, Content.transform);
            SlotStuff(item, isUse, mSlot);
            return;
        }
        else
        {
            foreach (Slot slot in Slots)
            {
                if (slot.IsFull == false)
                {
                    ItemEqual(slot, item, isUse);
                    return;
                }
                else
                {
                    Slot mSlot = Instantiate(Slot, Content.transform);
                    SlotStuff(item, isUse, mSlot);
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

    public void ItemDrop(Slot slot,GameObject ItemObject)
    {
        GameObject Item = Instantiate(ItemObject);
        Item.transform.parent = GameManager.GunSlot.Blank;
        Item.transform.position = new Vector3(GameManager.Character.transform.position.x + 2, GameManager.Character.transform.position.y, 1);
        Destroy(slot.gameObject);
        Slots.Remove(slot);
    }

    #endregion
}