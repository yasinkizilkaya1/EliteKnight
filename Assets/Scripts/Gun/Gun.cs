using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string TAG_SUPPORT = "Support";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_TOGGLE = "Toggle";

    #endregion

    #region Fields

    private GameManager gameManager;
    public Character character;
    public gun gun;

    public List<GameObject> BarrelList;
    public GameObject AmmoPrefabObject;
    public GameObject clipObject;

    public int SpareBulletCount;
    public int ClipCapacity;
    private int mCurrentAmmo;

    public bool isWeaponReload;
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
        ClipReloadEnum();
    }

    #endregion

    #region Public Method

    public void GunFire()
    {
        if (mCurrentAmmo == 0)
        {
            IsCanShoot = false;
        }

        if (mCurrentAmmo > 0 && IsCanShoot)
        {
            for (int i = BarrelList.Count - 1; i >= 0; i--)
            {
                mCurrentAmmo--;
                GameObject Bullet = Instantiate(AmmoPrefabObject, BarrelList[i].transform.position, BarrelList[i].transform.rotation) as GameObject;
                Bullet.GetComponent<Bullet>().weapon = gameObject.GetComponent<Gun>();
                gameManager.ammoBar.BarImageList[mCurrentAmmo].color = Color.grey;
            }
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
        if (mCurrentAmmo == 0 && mWeaponReload == gun.ReloadTime && SpareBulletCount > 0)
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
        mWeaponReload = gun.ReloadTime;
        ClipCapacity = gun.ClipCapacity;
        mCurrentAmmo = ClipCapacity;
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

                mWeaponReload = gun.ReloadTime;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while (true)
        {
            if (mCurrentAmmo <= ClipCapacity && mCurrentAmmo != SpareBulletCount)
            {
                yield return new WaitForSeconds(mFillingAmount);
                mCurrentAmmo++;

                if (mCurrentAmmo <= ClipCapacity)
                {
                    gameManager.ammoBar.BarImageList[mCurrentAmmo - 1].color = Color.black;
                }
            }
            else
                break;
        }
    }

    #endregion
}