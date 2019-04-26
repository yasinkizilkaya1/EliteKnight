using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string TAG_UIMANAGER = "UIManager";
    private const string TAG_CHARACETER = "Character";

    #endregion

    #region Fields

    public UIManager mUIManager;
    public Character character;
    public Weapon weapon;
    private GameManager mGameManager;
    private KeySettings mKeySettings;

    public List<GameObject> Barrels;
    public GameObject AmmoPrefabObject;
    public GameObject clipObject;

    public int SpareBulletCount;
    public int CurrentAmmo;

    public bool isWeaponReload;
    public bool IsCanShoot;

    public float Range;
    private float mWeaponReload;
    private float mFillingAmount;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ClipReload();
    }

    #endregion

    #region Public Method

    public void ClipReload()
    {
        if (Input.GetKeyDown(mKeySettings.Keys[5].CurrentKey) && mGameManager.isPause == false && isWeaponReload == false && CurrentAmmo != weapon.ClipCapacity && SpareBulletCount > 0 && weapon.IsAttak)
        {
            mUIManager.ammoBar.ReloadGUIObject.SetActive(true);
            GunClipDrup();
            Instantiate(clipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / weapon.ClipCapacity;
            SpareBulletCount -= weapon.ClipCapacity - CurrentAmmo;
            isWeaponReload = true;
            WeaponReload();
        }
        else
        {
            WeaponReload();
        }
    }

    public void AutoWeaponReload()
    {
        if (CurrentAmmo == 0 && mWeaponReload == weapon.ReloadTime && SpareBulletCount > 0)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / weapon.ClipCapacity;
            SpareBulletCount -= weapon.ClipCapacity - CurrentAmmo;
            isWeaponReload = true;
            WeaponReload();
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        if (SpareBulletCount == 0)
        {
            SpareBulletCount = weapon.TotalBullet;
        }
        else
        {
            if (CurrentAmmo != weapon.ClipCapacity)
            {
                SpareBulletCount -= CurrentAmmo;
            }
        }

        IsCanShoot = true;
        isWeaponReload = false;
        CurrentAmmo = weapon.ClipCapacity;
        mWeaponReload = weapon.ReloadTime;
        Range = weapon.Range;
        mUIManager = GameObject.FindWithTag(TAG_UIMANAGER).GetComponent<UIManager>();
        mGameManager = mUIManager.GameManager;
        mKeySettings = mGameManager.KeySettings;
    }

    private void WeaponReload()
    {
        if (mWeaponReload > 0 && isWeaponReload)
        {
            mWeaponReload -= Time.deltaTime;
            CurrentAmmo = 0;
            IsCanShoot = false;

            if (mWeaponReload >= 0.2 && SpareBulletCount >= CurrentAmmo)
            {
                StartCoroutine(BulletReloadEnum());
            }

            if (mWeaponReload <= 0.2)
            {
                IsCanShoot = true;
                isWeaponReload = false;
                StopAllCoroutines();

                if (SpareBulletCount < weapon.ClipCapacity)
                {
                    CurrentAmmo = SpareBulletCount;
                    SpareBulletCount = 0;
                }
                else
                {
                    CurrentAmmo = weapon.ClipCapacity;
                }

                mWeaponReload = weapon.ReloadTime;
                mUIManager.ammoBar.ReloadGUIObject.SetActive(false);
                mUIManager.ammoBar.ClipAmountText.text = SpareBulletCount.ToString();
            }
        }
    }

    private void GunClipDrup()
    {
        for (int i = 0; i < CurrentAmmo; i++)
        {
            mUIManager.ammoBar.BarImages[i].color = Color.grey;
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while ((CurrentAmmo <= weapon.ClipCapacity && CurrentAmmo != SpareBulletCount))
        {
            yield return new WaitForSeconds(mFillingAmount);
            CurrentAmmo++;

            if (CurrentAmmo <= weapon.ClipCapacity)
            {
                mUIManager.ammoBar.BarImages[CurrentAmmo - 1].color = Color.black;
            }
        }
    }

    #endregion
}