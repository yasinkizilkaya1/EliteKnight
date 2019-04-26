using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    #region Constants

    private const string TAG_SUPPORT = "Support";
    private const int MAX_VALUE = 100;

    #endregion

    #region Fields

    public Toggle AutoClipReloadToggle;

    public GameObject PlayerItemPanelObject;
    public GameObject ClipObject;
    public GameObject LoadingPanelObject;
    public GameObject LoadingSliderObject;
    public GameObject AutoReloadObject;
    public GameObject GunSlotObject;
    public GameObject PanelClip;
    public GameObject PanelRight;

    public Text LoadingText;
    public GameManager GameManager;
    public AmmoBar ammoBar;
    public Slider slider;

    public List<Toggle> Toggles;
    public List<Text> ButtonTexts;

    public Slider HealthBarSlider;
    public Slider ArmorBarSlider;
    public Slider EnergyBarSlider;

    public Map Map;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        StartCoroutine(Loading());
        slider.maxValue = MAX_VALUE;
    }

    #endregion

    #region Enumerator Methods

    private IEnumerator Loading()
    {
        bool spawn = false;

        while (true)
        {
            if (slider.value <= MAX_VALUE)
            {
                yield return new WaitForSeconds(0.01f);
                slider.value++;

                if (slider.value == MAX_VALUE)
                {
                    GunSlotObject.SetActive(true);
                    LoadingPanelObject.SetActive(false);
                    PlayerItemPanelObject.SetActive(true);

                    if (GameManager.CharacterData.Name == TAG_SUPPORT)
                    {
                        AutoReloadObject.SetActive(false);
                        ClipObject.SetActive(false);
                    }
                    else
                    {
                        ClipObject.SetActive(true);
                        AutoReloadObject.SetActive(true);
                    }

                    if (GameManager.CharacterData != null && spawn == false)
                    {
                        Map.CharacterSpawn();
                        StartCoroutine(Bars());
                        spawn = true;
                        StopCoroutine(Loading());
                    }
                }
            }
            else
                break;
        }
    }

    public IEnumerator GameOver()
    {
        LoadingPanelObject.SetActive(true);
        LoadingSliderObject.SetActive(false);
        PanelClip.SetActive(false);
        PanelRight.SetActive(false);
        LoadingText.text = "GAME OVER";
        LoadingText.fontSize = 60;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(GameManager.TAG_LOBBY);
    }

    private IEnumerator Bars()
    {
        yield return new WaitForSeconds(0.1f);
        EnergyBarSlider.gameObject.SetActive(true);
        HealthBarSlider.maxValue = GameManager.CharacterData.MaxHealth;
        HealthBarSlider.value = GameManager.CharacterData.Health;
        ArmorBarSlider.maxValue = GameManager.CharacterData.Defence;
        ArmorBarSlider.value = GameManager.CharacterData.Defence;
        EnergyBarSlider.maxValue = GameManager.CharacterData.MaxEnergy;
        EnergyBarSlider.value = GameManager.CharacterData.Energy;
        StopAllCoroutines();
    }

    public IEnumerator SetActiveBars()
    {
        yield return new WaitForSeconds(0.2f);
        if (GameManager.Character.CurrentDefence != GameManager.Character.MaxDefance)
        {
            HealthBarSlider.gameObject.SetActive(true);
            ArmorBarSlider.gameObject.SetActive(true);
        }

        if (ArmorBarSlider.value == 0)
        {
            ArmorBarSlider.gameObject.SetActive(false);
        }
    }

    #endregion
}