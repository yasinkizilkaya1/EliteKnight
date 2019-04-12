using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";
    public const int numItemSlot = 1;

    #endregion

    #region Fields

    private GameManager mGameManager;
    private Character mCharacter;

    public Image[] ItemImage;
    public Item[] Items;

    private List<Gun> mGuns;

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
        mGameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
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

    public void GunAdd(Weapon weapon, GameObject RightWeaponObject)
    {
        bool isSameGunHolder = false;
        mGuns = mCharacter.Guns;

        foreach (Gun gun in mGuns)
        {
            if (weapon.Id == gun.weapon.Id)
            {
                gun.SpareBulletCount += weapon.TotalBullet;
                isSameGunHolder = true;
            }
        }

        if (isSameGunHolder == false)
        {
            Gun NewGun = Instantiate(weapon.ItemObject, RightWeaponObject.transform).GetComponent<Gun>() as Gun;
            NewGun.transform.localScale = new Vector3(10, 10, 1);
            NewGun.gameObject.SetActive(false);
            mGuns.Add(NewGun);
        }
    }

    public void GunChange(Gun gun)
    {
        if (mCharacter.Gun != gun && mCharacter.Gun.isWeaponReload == false)
        {
            mGameManager.UIManager.ammoBar.ClipAmountText.text = gun.SpareBulletCount.ToString();
            mGameManager.Character.IsNewGun = true;
            mCharacter.Gun.gameObject.SetActive(false);
            mCharacter.Gun = gun;
            gun.gameObject.SetActive(true);
            ItemImage[0].sprite = gun.weapon.Icon;
            Items[0] = mCharacter.Gun.weapon;
        }
    }

    public void GunDrop(Character character)
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

    #endregion

    #region IEnumerator Method

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

    #endregion
}