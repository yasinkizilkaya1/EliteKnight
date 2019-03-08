using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int numItemSlot = 3;

    public Image[] ItemImage = new Image[numItemSlot];
    public Item[] Items = new Item[numItemSlot];
    public Button[] itemDropButton = new Button[numItemSlot];

    public void ItemAdd(Item itemAdd)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = itemAdd;
                ItemImage[i].sprite = Items[i].Icon;
                ItemImage[i].enabled = true;
                return;
            }
        }
    }


    private void Start()
    {
        for (int index = 0; index < itemDropButton.Length; index++)
        {
          //  itemDropButton[index].onClick.AddListener(ItemDrop(index));
        }
    }

    public void ItemDrop(int index)
    {
        if (Items[index] != null)
        {
            Items[index] = null;
            ItemImage[index].sprite = null;
            ItemImage[index].enabled = false;
            return;
        }
    }
}