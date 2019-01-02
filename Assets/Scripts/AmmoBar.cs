using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Support";
    private const int Ammo_Bar_Background_Width = 250;

    #endregion

    #region Fields

    public GridLayoutGroup AmmoGridLayout;
    public List<Image> BarImageList;
    public GameManager gameManager;
    public Spawn spawn;
    public TiroNew tiroNew;
    public Image BarImage;

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
        if (spawn.listCharacterList[0].gun != null && spawn.listCharacterList[0].gun.tiroNew != null && tiroNew != null)
        {
            ClipAmountText.text = spawn.listCharacterList[0].gun.tiroNew.SpareBulletCount.ToString();

            if (tiroNew.isShoot == false && tiroNew.AutoClipReloadToggle.isOn == false && tiroNew.isWeaponReload == false)
            {
                ReloadGUIObject.SetActive(true);
            }
            else
            {
                ReloadGUIObject.SetActive(false);
            }

            if (spawn.listCharacterList[0].isAk47 || spawn.listCharacterList[0].isShotgun || spawn.listCharacterList[0].isGun)
            {
                StartCoroutine(Ammobar());
                spawn.listCharacterList[0].isGun = false;
            }
        }
        else
        {
            tiroNew = spawn.listCharacterList[0].gun.tiroNew;
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
        AmmoBarWidth = (Ammo_Bar_Background_Width / (AmmoCount + 1));
        AmmoBarGap = AmmoBarWidth / (AmmoCount + 1);

        AmmoGridLayout.cellSize = new Vector2(AmmoBarWidth, 100);
        AmmoGridLayout.spacing = new Vector2(AmmoBarGap + 0.5f, 0);

        for (int i = 0; i < AmmoCount; i++)
        {
            Image Bar = Instantiate(BarImage, transform);
            BarImageList.Add(Bar);
        }
    }

    private void AmmoBarDelete()
    {
        for (int i = 0; i < AmmoCount - 1; i++)
        {
            BarImageList[i].color = Color.grey;
        }
        BarImageList.Clear();
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

        if (spawn.listCharacterList[0].gun.tiroNew != null)
        {
            AmmoBarDelete();
            AmmoCount = spawn.listCharacterList[0].gun.tiroNew.ClipCapacity;
            yield return new WaitForSeconds(0.4f);
            AmmoBarsCreate();
        }
    }

    #endregion
}