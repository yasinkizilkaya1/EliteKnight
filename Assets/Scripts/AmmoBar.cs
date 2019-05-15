using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const int mAMMO_BAR_BACKGROUND_WIDTH = 85;
    private const int mAMMO_BAR_BACKGROUND_GAP = 6;

    #endregion

    #region Fields

    public GridLayoutGroup AmmoGridLayout;

    public UIManager UIManager;
    public GameManager GameManager;
    private Character Character;
    public Gun Gun;

    public Image BarImage;
    public List<Image> BarImages;
    public List<GameObject> BarImageObjects;
    public Animator Animator;

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
        if (Gun != null && UIManager.AutoClipReloadToggle.isOn)
        {
            Gun.AutoWeaponReload();
        }
        else
        {
            Gun = GameManager.Character.Gun;
        }

        if (Character.IsNewGun)
        {
            Gun = GameManager.Character.Gun;
            AmmoBarInstantlyFilling();
            GameManager.Character.IsNewGun = false;
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        BarImages = new List<Image>();
        BarImageObjects = new List<GameObject>();
        Character = GameManager.Character;
        StartCoroutine(Ammobar());
    }

    private void AmmoBarsCreate()
    {
        mAmmoBarWidth = mAMMO_BAR_BACKGROUND_WIDTH / mAmmoCount;
        mAmmoBarGap = (float)mAMMO_BAR_BACKGROUND_GAP / (mAmmoCount - 1);

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
        mAmmoCount = GameManager.Character.Gun.Weapon.ClipCapacity;
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
        if (Gun.CurrentAmmo != mAmmoCount)
        {
            for (int i = mAmmoCount - 1; i >= Gun.CurrentAmmo; i--)
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
            mAmmoCount = Gun.Weapon.ClipCapacity;
            AmmoBarsCreate();
        }
    }

    #endregion
}