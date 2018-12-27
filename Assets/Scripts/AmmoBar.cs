using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Support";

    #endregion

    #region Fields

    public GameManager gameManager;
    public Spawn spawn;

    public GameObject ReloadGUIObject;
    public GameObject SliderObject;

    public Slider AmmoSlider;

    public Text ClipAmountText;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (spawn.listCharacterList[0].gun != null && spawn.listCharacterList[0].gun.tiroNew != null)
        {
            ClipAmountText.text = spawn.listCharacterList[0].gun.tiroNew.SpareBulletCount.ToString() + "/" + spawn.listCharacterList[0].gun.tiroNew.Ammo.ToString();
            AmmoSlider.value = spawn.listCharacterList[0].gun.tiroNew.Ammo;


            if (spawn.listCharacterList[0].gun.tiroNew.isShoot == false)
            {
                ReloadGUIObject.SetActive(true);
            }
            else
            {
                ReloadGUIObject.SetActive(false);
            }

            if (spawn.listCharacterList[0].isFindAk47)
            {
                StartCoroutine(Ammobar());
            }
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        StartCoroutine(Ammobar());

        if (gameManager.SelectedCardNameString == TAG_CHARACTER)
        {
            SliderObject.SetActive(false);
            ReloadGUIObject.SetActive(false);
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Ammobar()
    {
        yield return new WaitForSeconds(0.1f);

        if (spawn.listCharacterList[0].gun.tiroNew != null)
        {
            AmmoSlider.maxValue = spawn.listCharacterList[0].gun.tiroNew.ClipCapacity;
        }
    }

    #endregion
}