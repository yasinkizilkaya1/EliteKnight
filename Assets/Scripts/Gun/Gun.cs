using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string mTAG_UIMANAGER = "UIManager";
    private const string mTAG_CHARACETER = "Character";

    #endregion

    #region Fields

    public UIManager mUIManager;
    public Character Character;
    public Weapon Weapon;
    private GameManager mGameManager;
    private KeySetting mKeySetting;

    public List<GameObject> Barrels;
    public GameObject ClipObject;

    public int SpareBulletCount;
    public int CurrentAmmo;

    public bool IsWeaponReload;
    public bool IsCanShoot;

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
        if (Input.GetKeyDown(mKeySetting.Keys[5].CurrentKey) && mGameManager.IsPause == false && IsWeaponReload == false && CurrentAmmo != Weapon.ClipCapacity && SpareBulletCount > 0 && Weapon.IsAttak)
        {
            ClipReload();
        }
        else
        {
            if (mWeaponReload > 0 && IsWeaponReload)
            {
                WeaponReload();
            }
        }
    }

    #endregion

    #region Public Method

    public void ClipReload()
    {
        GunClipDrup();
        Instantiate(ClipObject, transform.position, transform.rotation);
        mFillingAmount = (mWeaponReload - 0.4f) / Weapon.ClipCapacity;
        SpareBulletCount -= Weapon.ClipCapacity - CurrentAmmo;
        IsWeaponReload = true;

        if (mWeaponReload > 0 && IsWeaponReload)
        {
            WeaponReload();
        }
    }

    public void AutoWeaponReload()
    {
        if (CurrentAmmo == 0 && mWeaponReload == Weapon.ReloadTime && SpareBulletCount > 0)
        {
            Instantiate(ClipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / Weapon.ClipCapacity;
            SpareBulletCount -= Weapon.ClipCapacity - CurrentAmmo;
            IsWeaponReload = true;

            if (mWeaponReload > 0 && IsWeaponReload)
            {
                WeaponReload();
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        mUIManager = GameObject.FindWithTag(mTAG_UIMANAGER).GetComponent<UIManager>();

        mGameManager = mUIManager.GameManager;
        mKeySetting = mGameManager.KeySetting;
        IsCanShoot = true;
        IsWeaponReload = false;
        CurrentAmmo = Weapon.ClipCapacity;
        mWeaponReload = Weapon.ReloadTime;

        if (SpareBulletCount == 0)
        {
            SpareBulletCount = Weapon.TotalBullet;
        }
        else
        {
            if (CurrentAmmo != Weapon.ClipCapacity)
            {
                SpareBulletCount -= CurrentAmmo;
            }
        }
    }

    private void WeaponReload()
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
            IsWeaponReload = false;
            StopAllCoroutines();

            if (SpareBulletCount < Weapon.ClipCapacity)
            {
                CurrentAmmo = SpareBulletCount;
                SpareBulletCount = 0;
            }
            else
            {
                CurrentAmmo = Weapon.ClipCapacity;
            }

            mWeaponReload = Weapon.ReloadTime;
            mUIManager.AmmoBar.ClipAmountText.text = SpareBulletCount.ToString();
        }
    }

    private void GunClipDrup()
    {
        for (int i = 0; i < CurrentAmmo; i++)
        {
            mUIManager.AmmoBar.BarImages[i].color = Color.grey;
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while ((CurrentAmmo <= Weapon.ClipCapacity && CurrentAmmo != SpareBulletCount))
        {
            yield return new WaitForSeconds(mFillingAmount);
            CurrentAmmo++;

            if (CurrentAmmo <= Weapon.ClipCapacity)
            {
                mUIManager.AmmoBar.BarImages[CurrentAmmo - 1].color = Color.black;
            }
        }
    }

    #endregion
}