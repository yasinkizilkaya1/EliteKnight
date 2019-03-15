using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_CHARACETER = "Character";

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
                    gameManager.ammoBar.BarImageList[CurrentAmmo].color = Color.grey;
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
        if (Input.GetKeyDown(gameManager.ReloadEnum) && gameManager.isPause == false && isWeaponReload == false && CurrentAmmo != ClipCapacity && SpareBulletCount > 0 && weapon.IsAttak)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            mFillingAmount = (mWeaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount -= ClipCapacity - CurrentAmmo;
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
            mFillingAmount = (mWeaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount -= ClipCapacity - CurrentAmmo;
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
        CurrentAmmo = ClipCapacity;
        Range = weapon.Range;
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        character = gameManager.spawn.CharacterList[0];
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

                if (SpareBulletCount < ClipCapacity)
                {
                    CurrentAmmo = SpareBulletCount;
                    SpareBulletCount = 0;
                }
                else
                {
                    CurrentAmmo = ClipCapacity;
                }

                mWeaponReload = weapon.ReloadTime;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while ((CurrentAmmo <= ClipCapacity && CurrentAmmo != SpareBulletCount))
        {
            yield return new WaitForSeconds(mFillingAmount);
            CurrentAmmo++;

            if (CurrentAmmo <= ClipCapacity)
            {
                gameManager.ammoBar.BarImageList[CurrentAmmo - 1].color = Color.black;
            }
        }
    }

    #endregion
}