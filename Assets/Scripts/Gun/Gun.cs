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

    private UIManager mUIManager;
    public Character character;
    public Weapon weapon;

    public List<GameObject> BarrelList;
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

    public void Fire()
    {
        if (CurrentAmmo > 0 && IsCanShoot == true && weapon.IsAttak)
        {
            for (int i = BarrelList.Count - 1; i >= 0; i--)
            {
                CurrentAmmo--;
                GameObject Bullet = ObjectPooler.SharedInstance.GetPooledObject("bullet");

                if (Bullet != null)
                {
                    Bullet.transform.position = BarrelList[i].transform.position;
                    Bullet.transform.rotation = BarrelList[i].transform.rotation;
                    Bullet.SetActive(true);
                    Bullet.GetComponent<Bullet>().weapon = gameObject.GetComponent<Gun>().weapon;
                    mUIManager.ammoBar.BarImageList[CurrentAmmo].color = Color.grey;
                }
            }
        }
        else
        {
            IsCanShoot = false;
        }
    }

    public void ClipReload()
    {
        if (Input.GetKeyDown(mUIManager.ReloadEnum) && mUIManager.GameManager.isPause == false && isWeaponReload == false && CurrentAmmo != weapon.ClipCapacity && SpareBulletCount > 0 && weapon.IsAttak)
        {
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
        IsCanShoot = true;
        isWeaponReload = false;
        mWeaponReload = weapon.ReloadTime;
        CurrentAmmo = weapon.ClipCapacity;
        Range = weapon.Range;
        mUIManager = GameObject.FindWithTag(TAG_UIMANAGER).GetComponent<UIManager>();
        character = mUIManager.spawn.CharacterList[0];
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
            }
        }
    }

    private void GunClipDrup()
    {
        for (int i = 0; i < CurrentAmmo; i++)
        {
            character.UIManager.ammoBar.BarImageList[i].color = Color.grey;
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
                mUIManager.ammoBar.BarImageList[CurrentAmmo - 1].color = Color.black;
            }
        }
    }

    #endregion
}