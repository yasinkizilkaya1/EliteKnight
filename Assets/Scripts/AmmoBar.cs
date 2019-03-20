using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const int AMMO_BAR_BACKGROUND_WIDTH = 250;

    #endregion

    #region Fields

    public GridLayoutGroup AmmoGridLayout;

    public GameManager gameManager;
    public Spawn spawn;
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
            ClipAmountText.text = spawn.CharacterList[0].Gun.SpareBulletCount.ToString();

            if (gun.IsCanShoot == false && gameManager.AutoClipReloadToggle.isOn == false && gun.isWeaponReload == false)
            {
                ReloadGUIObject.SetActive(true);
            }
            else
            {
                ReloadGUIObject.SetActive(false);
            }
        }
        else
        {
            gun = spawn.CharacterList[0].Gun;
        }

        if (spawn.CharacterList[0].IsNewGun)
        {
            gun = spawn.CharacterList[0].Gun;
            AmmoBarInstantlyFilling();
            spawn.CharacterList[0].IsNewGun = false;
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
        mAmmoBarWidth = (AMMO_BAR_BACKGROUND_WIDTH / (mAmmoCount + 1));
        mAmmoBarGap = mAmmoBarWidth / (mAmmoCount + 1);

        AmmoGridLayout.cellSize = new Vector2(mAmmoBarWidth, 100);
        AmmoGridLayout.spacing = new Vector2(mAmmoBarGap + 0.5f, 0);

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
        mAmmoCount = spawn.CharacterList[0].Gun.weapon.ClipCapacity;
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

        if (spawn.CharacterList[0].Gun != null)
        {
            AmmoBarDelete();
            mAmmoCount = gun.weapon.ClipCapacity;
            AmmoBarsCreate();
        }
    }

    #endregion
}