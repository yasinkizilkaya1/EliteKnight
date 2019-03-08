﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    private GameManager gameManager;
    public Character character;
    public Weapon weapon;

    public List<GameObject> BarrelList;
    public GameObject AmmoPrefabObject;
    public GameObject clipObject;

    public int SpareBulletCount;
    public int ClipCapacity;
    private int mCurrentAmmo;

    public bool isWeaponReload;
    public bool IsCanShoot;

    public float Range;
    private float mWeaponReload;
    private float mFillingAmount;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        ClipReloadEnum();
    }

    #endregion

    #region Public Method

    public void Fire()
    {
        if (mCurrentAmmo > 0 && IsCanShoot == true)
        {
            for (int i = BarrelList.Count - 1; i >= 0; i--)
            {
                mCurrentAmmo--;
                GameObject Bullet = ObjectPooler.SharedInstance.GetPooledObject("bullet");

                if (Bullet != null)
                {
                    Bullet.transform.position = BarrelList[i].transform.position;
                    Bullet.transform.rotation = BarrelList[i].transform.rotation;
                    Bullet.SetActive(true);
                    Bullet.GetComponent<Bullet>().weapon = gameObject.GetComponent<Gun>().weapon;
                    gameManager.ammoBar.BarImageList[mCurrentAmmo].color = Color.grey;
                }
            }
        }
        else
        {
            IsCanShoot = false;
        }
    }

    public void ClipReloadEnum()
    {
        if (Input.GetKeyDown(gameManager.ReloadEnum) && gameManager.isPause == false && isWeaponReload == false && mCurrentAmmo != ClipCapacity && SpareBulletCount > 0)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount -= ClipCapacity - mCurrentAmmo;
            isWeaponReload = true;
            WeaponReload();
        }
        else
        {
            WeaponReload();
        }
    }

    public void AutoWeaponReloadEnum()
    {
        if (mCurrentAmmo == 0 && mWeaponReload == weapon.ReloadTime && SpareBulletCount > 0)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount -= ClipCapacity - mCurrentAmmo;
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
        ClipCapacity = weapon.ClipCapacity;
        mCurrentAmmo = ClipCapacity;
        Range = weapon.Range;
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        character = gameManager.spawn.CharacterList[0];
    }

    private void WeaponReload()
    {
        if (mWeaponReload > 0 && isWeaponReload)
        {
            mWeaponReload -= Time.deltaTime;
            mCurrentAmmo = 0;
            IsCanShoot = false;

            if (mWeaponReload >= 0.2 && SpareBulletCount >= mCurrentAmmo)
            {
                StartCoroutine(BulletReloadEnum());
            }

            if (mWeaponReload <= 0.2)
            {
                IsCanShoot = true;
                isWeaponReload = false;
                StopAllCoroutines();

                if (SpareBulletCount < ClipCapacity)
                {
                    mCurrentAmmo = SpareBulletCount;
                    SpareBulletCount = 0;
                }
                else
                {
                    mCurrentAmmo = ClipCapacity;
                }

                mWeaponReload = weapon.ReloadTime;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while ((mCurrentAmmo <= ClipCapacity && mCurrentAmmo != SpareBulletCount))
        {
            yield return new WaitForSeconds(mFillingAmount);
            mCurrentAmmo++;

            if (mCurrentAmmo <= ClipCapacity)
            {
                gameManager.ammoBar.BarImageList[mCurrentAmmo - 1].color = Color.black;
            }
        }
    }

    #endregion
}