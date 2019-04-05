using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const int AMMO_BAR_BACKGROUND_WIDTH = 85;
    private const int AMMO_BAR_BACKGROUND_GAP = 6;

    #endregion

    #region Fields

    public GridLayoutGroup AmmoGridLayout;

    public UIManager UIManager;
    public Gun gun;

    public Image BarImage;
    public List<Image> BarImageList;
    public List<GameObject> BarImageListObject;
    public GameObject ReloadGUIObject;

    public Text ClipAmountText;

    private float mAmmoBarGap;

    private int mAmmoBarWidth;
    private int mAmmoCount;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (gun != null)
        {
            ClipAmountText.text = UIManager.GameManager.Character.Gun.SpareBulletCount.ToString();

            if (gun.IsCanShoot == false && UIManager.AutoClipReloadToggle.isOn == false && gun.isWeaponReload == false)
            {
                ReloadGUIObject.SetActive(true);
            }
            else
            {
                ReloadGUIObject.SetActive(false);
            }

            if (UIManager.AutoClipReloadToggle.isOn)
            {
                gun.AutoWeaponReload();
            }
        }
        else
        {
            gun =UIManager.GameManager.Character.Gun;
        }

        if (UIManager.GameManager.Character.IsNewGun)
        {
            gun = UIManager.GameManager.Character.Gun;
            AmmoBarInstantlyFilling();
            UIManager.GameManager.Character.IsNewGun = false;
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        StartCoroutine(Ammobar());
    }

    private void AmmoBarsCreate()
    {
        mAmmoBarWidth = AMMO_BAR_BACKGROUND_WIDTH / mAmmoCount;
        mAmmoBarGap =(float)AMMO_BAR_BACKGROUND_GAP / (mAmmoCount - 1);

        AmmoGridLayout.cellSize = new Vector2(mAmmoBarWidth, 80);
        AmmoGridLayout.spacing = new Vector2(mAmmoBarGap, 0);

        for (int i = 0; i < mAmmoCount; i++)
        {
            Image Bar = Instantiate(BarImage, transform);
            BarImageListObject.Add(Bar.gameObject);
            BarImageList.Add(Bar);
        }
    }

    private void AmmoBarInstantlyFilling()
    {
        AmmoBarDelete();
        mAmmoCount = UIManager.GameManager.Character.Gun.weapon.ClipCapacity;
        AmmoBarsCreate();
        AmmoBarsBackup();
    }

    private void AmmoBarDelete()
    {
        for (int i = 0; i <= mAmmoCount - 1; i++)
        {
            Destroy(BarImageListObject[i]);
        }
        BarImageList.Clear();
        BarImageListObject.Clear();
    }

    private void AmmoBarsBackup()
    {
        if (gun.CurrentAmmo != mAmmoCount)
        {
            for (int i = mAmmoCount - 1; i >= gun.CurrentAmmo; i--)
            {
                BarImageList[i].color = Color.grey;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Ammobar()
    {
        yield return new WaitForSeconds(0.1f);

        if (UIManager.GameManager.Character.Gun != null)
        {
            AmmoBarDelete();
            mAmmoCount = gun.weapon.ClipCapacity;
            AmmoBarsCreate();
        }
    }

    #endregion
}