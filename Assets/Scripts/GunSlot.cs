using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    #region Constants

    private const string mTAG_GAMEMANAGER = "GameManager";
    public const int numItemSlot = 1;

    #endregion

    #region Fields

    private GameManager mGameManager;
    private Character mCharacter;

    public Image[] ItemImage;
    public Item[] Items;

    private List<Gun> mGuns;
    public Transform Blank;

    #endregion

    #region Unity Method

    private void Start()
    {
        Initialize();
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        mGameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        mCharacter = mGameManager.Character;
        StartCoroutine(GunSlotActive());
    }

    #endregion

    #region Public Methods 

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

    public void GunAdd(GameObject gunObject, GameObject rightWeaponObject)
    {
        mGuns = mCharacter.Guns;
        gunObject.transform.SetParent(rightWeaponObject.transform, false);
        gunObject.transform.position = mGuns[0].transform.position;
        gunObject.transform.rotation = mGuns[0].transform.rotation;
        gunObject.transform.localScale = mGuns[0].transform.localScale;
        mGuns.Add(gunObject.GetComponent<Gun>());
    }

    public void GunChange(Gun gun, Character character)
    {
        if (character.Gun != gun && character.Gun.IsWeaponReload == false)
        {
            character.IsNewGun = true;
            character.Gun.gameObject.SetActive(false);
            character.Gun = gun;
            gun.gameObject.SetActive(true);
            ItemImage[0].sprite = gun.Weapon.Icon;
            Items[0] = character.Gun.Weapon;
        }
    }

    public void GunDrop(Character character)
    {
        if (character.Gun != null && character.Guns.Count > 1)
        {
            foreach (Gun gun in character.Guns)
            {
                if (gun == character.Gun)
                {
                    for (int i = 0; i < mGameManager.Inventory.Slots.Count; i++)
                    {
                        if (mGameManager.Inventory.Slots[i].Item == character.Gun.Weapon)
                        {
                            Destroy(mGameManager.Inventory.Slots[i].gameObject);
                            mGameManager.Inventory.Slots.Remove(mGameManager.Inventory.Slots[i]);
                        }
                    }
                    gun.transform.parent = Blank;
                    gun.transform.position = new Vector3(character.transform.position.x + 2, character.transform.position.y, 1);
                    character.Guns.Remove(character.Gun);
                    character.Gun = character.Guns[character.Guns.Count - 1];
                    character.IsNewGun = true;
                    character.Gun.gameObject.SetActive(true);
                    ItemImage[0].sprite = character.Gun.Weapon.Icon;
                    return;
                }
            }
        }
    }

    public void GunDrop(Character character, Item item, Slot slot)
    {
        if (character.Gun != null && character.Guns.Count > 1)
        {
            Gun Gun = null;

            foreach (Gun gun in character.Guns)
            {
                if (item == gun.Weapon)
                {
                    Gun = gun;
                    Destroy(slot.gameObject);
                    mGameManager.Inventory.Slots.Remove(slot);
                }
            }

            Gun.transform.parent = Blank;
            Gun.transform.position = new Vector3(character.transform.position.x + 2, character.transform.position.y, 1);
            Gun.gameObject.SetActive(true);
            character.Guns.Remove(Gun);

            if (Gun == character.Gun)
            {
                character.Gun = character.Guns[character.Guns.Count - 1];
                character.IsNewGun = true;
                character.Gun.gameObject.SetActive(true);
                ItemImage[0].sprite = character.Gun.Weapon.Icon;
            }
            return;
        }
    }

    #endregion

    #region IEnumerator Method

    IEnumerator GunSlotActive()
    {
        yield return new WaitForSeconds(0.2f);

        for (int index = 0; index < mGameManager.Character.Guns.Count; index++)
        {
            ItemAdd(mGameManager.Character.Guns[index].Weapon);
        }

        yield return new WaitForSeconds(0.1f);
        StopCoroutine(GunSlotActive());
    }

    #endregion
}