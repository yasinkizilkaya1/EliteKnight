using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    private const string TAG_GAMEMANAGER = "GameManager";
    public const int numItemSlot = 1;

    private GameManager mGameManager;

    public Image[] ItemImage;
    public Item[] Items;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        mGameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();

        StartCoroutine(GunSlotActive());
    }

    public void ItemAdd(Weapon item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                ItemImage[i].sprite = item.Icon;
                ItemImage[i].enabled = true;
                return;
            }
        }
    }

    public void ItemDrop(Character character)
    {
        if (character.Gun != null && character.Guns.Count > 1)
        {
            GameObject weapon = Instantiate(character.Gun.gameObject, new Vector3(character.transform.position.x + 2, character.transform.position.y, 1), Quaternion.identity);
            weapon.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            Destroy(character.Gun.gameObject);
            character.Guns.Remove(character.Gun);
            character.Gun = character.Guns[character.Guns.Count - 1];
            character.IsNewGun = true;
            character.Guns[character.Guns.Count - 1].gameObject.SetActive(true);
            ItemImage[0].sprite = character.Gun.weapon.Icon;
            return;
        }
    }

    IEnumerator GunSlotActive()
    {
        yield return new WaitForSeconds(0.2f);

        for (int index = 0; index < mGameManager.Character.Guns.Count; index++)
        {
            ItemAdd(mGameManager.Character.Guns[index].weapon);
        }

        yield return new WaitForSeconds(0.1f);
        StopCoroutine(GunSlotActive());
    }
}