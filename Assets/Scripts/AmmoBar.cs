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
    public GameManager GameManager;
    private Character Character;
    public Gun gun;

    public Image BarImage;
    public List<Image> BarImages;
    public List<GameObject> BarImageObjects;
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
        if (gun != null && UIManager.AutoClipReloadToggle.isOn)
        {
            gun.AutoWeaponReload();
        }
        else
        {
            gun = GameManager.Character.Gun;
        }

        if (Character.IsNewGun)
        {
            gun = GameManager.Character.Gun;
            AmmoBarInstantlyFilling();
            GameManager.Character.IsNewGun = false;
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        Character = GameManager.Character;
        StartCoroutine(Ammobar());
    }

    private void AmmoBarsCreate()
    {
        mAmmoBarWidth = AMMO_BAR_BACKGROUND_WIDTH / mAmmoCount;
        mAmmoBarGap = (float)AMMO_BAR_BACKGROUND_GAP / (mAmmoCount - 1);

        AmmoGridLayout.cellSize = new Vector2(mAmmoBarWidth, 80);
        AmmoGridLayout.spacing = new Vector2(mAmmoBarGap, 0);

        for (int i = 0; i < mAmmoCount; i++)
        {
            Image Bar = Instantiate(BarImage, transform);
            BarImageObjects.Add(Bar.gameObject);
            BarImages.Add(Bar);
        }
    }

    private void AmmoBarInstantlyFilling()
    {
        AmmoBarDelete();
        mAmmoCount = GameManager.Character.Gun.weapon.ClipCapacity;
        ClipAmountText.text = GameManager.Character.Gun.SpareBulletCount.ToString();
        AmmoBarsCreate();
        AmmoBarsBackup();
    }

    private void AmmoBarDelete()
    {
        for (int i = 0; i <= mAmmoCount - 1; i++)
        {
            Destroy(BarImageObjects[i]);
        }
        BarImages.Clear();
        BarImageObjects.Clear();
    }

    private void AmmoBarsBackup()
    {
        if (gun.CurrentAmmo != mAmmoCount)
        {
            for (int i = mAmmoCount - 1; i >= gun.CurrentAmmo; i--)
            {
                BarImages[i].color = Color.grey;
            }
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Ammobar()
    {
        yield return new WaitForSeconds(0.1f);

        if (GameManager.Character.Gun != null)
        {
            AmmoBarDelete();
            mAmmoCount = gun.weapon.ClipCapacity;
            AmmoBarsCreate();
        }
    }

    #endregion
}