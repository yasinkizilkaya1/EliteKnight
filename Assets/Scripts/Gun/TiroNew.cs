﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TiroNew : MonoBehaviour
{
    #region Constants

    private const float WEAPON_RELOAD_TİME = 3f;
    private const string TAG_WALL = "wall";
    private const string TAG_CHEST = "chest";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_TOGGLE = "Toggle";

    #endregion

    #region Fields

    private GameManager gameManager;
    public Toggle AutoClipReloadToggle;

    public GameObject AmmoPrefabObject;
    public GameObject clipObject;

    public int SpareBulletCount;
    public int ClipCapacity;
    public int Ammo;

    public bool isbarrel;
    public bool isBulletPos;
    public bool isWeaponReload;
    public bool isShoot;

    public float weaponReload;
    public float Velocidade;
    public float FillingAmount;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isBulletPos)
        {
            transform.Translate(Vector2.right * -Velocidade * Time.deltaTime);
        }

        if (isbarrel)
        {
            if (Input.GetButtonDown("Fire1") && Ammo > 0 && isShoot && gameManager.isPause == false)
            {
                Instantiate(AmmoPrefabObject, transform.position, transform.rotation);
                Ammo--;
            }
        }

        if (Ammo == 0)
        {
            isShoot = false;
        }

        ClipReloadEnum();
        AutoWeaponReloadEnum(AutoClipReloadToggle.isOn);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_WALL) || col.gameObject.CompareTag(TAG_CHEST))
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        isWeaponReload = false;
        weaponReload = WEAPON_RELOAD_TİME;
        isShoot = true;
        AutoClipReloadToggle = GameObject.FindWithTag(TAG_TOGGLE).GetComponent<Toggle>();
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        Ammo = ClipCapacity;
    }

    private void ClipReloadEnum()
    {
        if (Input.GetKeyDown(gameManager.ReloadEnum) && isWeaponReload == false && Ammo != ClipCapacity && gameManager.isPause == false && SpareBulletCount > 0)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            FillingAmount = (weaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount += Ammo;
            isWeaponReload = true;
            WeaponReload();
        }
        else
        {
            WeaponReload();
        }
    }

    private void AutoWeaponReloadEnum(bool isOn)
    {
        if (isOn == true && Ammo == 0 && weaponReload == WEAPON_RELOAD_TİME && gameManager.isPause == false && SpareBulletCount > 0)
        {
            Instantiate(clipObject, transform.position, transform.rotation);
            FillingAmount = (weaponReload - 0.4f) / ClipCapacity;
            SpareBulletCount += Ammo;
            isWeaponReload = true;
            WeaponReload();
        }
    }

    private void WeaponReload()
    {
        if (weaponReload > 0 && isWeaponReload)
        {
            weaponReload -= Time.deltaTime;
            Ammo = 0;
            isShoot = false;

            if (weaponReload >= 0.2 && SpareBulletCount >= Ammo)
            {
                StartCoroutine(BulletReloadEnum());
            }

            if (weaponReload <= 0.2)
            {
                StopAllCoroutines();
                isShoot = true;
                isWeaponReload = false;

                if (SpareBulletCount >= ClipCapacity)
                {
                    Ammo = ClipCapacity;
                    SpareBulletCount -= ClipCapacity;
                }
                else
                {
                    Ammo = SpareBulletCount;
                    SpareBulletCount = 0;
                }

                weaponReload = WEAPON_RELOAD_TİME;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator BulletReloadEnum()
    {
        while (true)
        {
            if (Ammo <= ClipCapacity)
            {
                yield return new WaitForSeconds(FillingAmount);
                Ammo++;
            }
            else
                break;
        }
    }

    #endregion
}