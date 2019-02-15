using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Support";
    private const int AMMO_BAR_BACKGROUND_WIDTH = 250;

    #endregion

    #region Fields

    public GridLayoutGroup AmmoGridLayout;

    public GameManager gameManager;
    public Spawn spawn;
    public TiroNew tiroNew;

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
        if (spawn.CharacterList[0].gun != null && spawn.CharacterList[0].gun.tiroNew != null && tiroNew != null)
        {
            ClipAmountText.text = spawn.CharacterList[0].gun.tiroNew.SpareBulletCount.ToString();

            if (tiroNew.isShoot == false && tiroNew.AutoClipReloadToggle.isOn == false && tiroNew.isWeaponReload == false)
            {
                ReloadGUIObject.SetActive(true);
            }
            else
            {
                ReloadGUIObject.SetActive(false);
            }

            if (spawn.CharacterList[0].isAk47 || spawn.CharacterList[0].isShotgun || spawn.CharacterList[0].isGun)
            {
                StartCoroutine(Ammobar());
                spawn.CharacterList[0].isGun = false;
            }
        }
        else
        {
            tiroNew = spawn.CharacterList[0].gun.tiroNew;
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        if (gameManager.SelectedCardNameString != TAG_CHARACTER)
        {
            StartCoroutine(Ammobar());
        }
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

        if (spawn.CharacterList[0].gun.tiroNew != null)
        {
            AmmoBarDelete();
            yield return new WaitForSeconds(0.2f);
            AmmoCount = spawn.CharacterList[0].gun.tiroNew.ClipCapacity;
            yield return new WaitForSeconds(0.2f);
            AmmoBarsCreate();
        }
    }

    #endregion
}