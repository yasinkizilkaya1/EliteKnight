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

    private float AmmoBarGap;

    private int AmmoBarWidth;
    private int AmmoCount;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (gun != null)
        {
            ClipAmountText.text = spawn.CharacterList[0].gun.SpareBulletCount.ToString();

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
            gun = spawn.CharacterList[0].gun;
        }

        if (spawn.CharacterList[0].IsNewGun)
        {
            StartCoroutine(Ammobar());
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
        AmmoBarWidth = (AMMO_BAR_BACKGROUND_WIDTH / (AmmoCount + 1));
        AmmoBarGap = AmmoBarWidth / (AmmoCount + 1);

        AmmoGridLayout.cellSize = new Vector2(AmmoBarWidth, 100);
        AmmoGridLayout.spacing = new Vector2(AmmoBarGap + 0.5f, 0);

        for (int i = 0; i < AmmoCount; i++)
        {
            Image Bar = Instantiate(BarImage, transform);
            BarImageListObject.Add(Bar.gameObject);
            BarImageList.Add(Bar);
        }
    }

    private void AmmoBarDelete()
    {
        for (int i = 0; i <= AmmoCount - 1; i++)
        {
            Destroy(BarImageListObject[i]);
        }
        BarImageList.Clear();
        BarImageListObject.Clear();
    }

    private void AmmoBarsBackup()
    {
        for (int i = 0; i < AmmoCount - 1; i++)
        {
            BarImageList[i].color = Color.black;
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Ammobar()
    {
        yield return new WaitForSeconds(0.1f);

        if (spawn.CharacterList[0].gun != null)
        {
            AmmoBarDelete();
            yield return new WaitForSeconds(0.2f);
            AmmoCount = spawn.CharacterList[0].gun.ClipCapacity;
            yield return new WaitForSeconds(0.2f);
            AmmoBarsCreate();
        }
    }

    #endregion
}