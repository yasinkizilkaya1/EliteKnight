using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Constants

    public const string TAG_LOBBY = "Lobby";

    #endregion

    #region Fields

    public GameObject PauseMenuObject;
    public GameObject PanelSettingObject;
    public GameObject PanelPauseObject;
    public GameObject PanelClipObject;

    public UIManager UIManager;
    public CharacterData CharacterData;
    public Character Character;
    public GunSlot GunSlot;

    public bool isPause;
    public bool isPlayerDead;

    public KeySettings KeySettings;
    public List<string> keycaps;


    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void OnGUI()
    {
        if (isPause == true)
        {
            for (int i = 0; i < UIManager.ButtonTexts.Count; i++)
            {
                if (UIManager.Toggles[i].isOn == true)
                {
                    KeysChange(i);
                }
            }
        }
    }

    private void Update()
    {
        if (Character != null)
        {
            if (Character.CurrentHP == 0)
            {
                isPlayerDead = true;
                Character.isDead = true;
                Destroy(this.gameObject);
                StartCoroutine(UIManager.GameOver());
                UIManager.HealthBarSlider.gameObject.SetActive(false);
            }
            else
            {
                UIManager.HealthBarSlider.value = Character.CurrentHP;
                UIManager.ArmorBarSlider.value = Character.CurrentDefence;
                UIManager.EnergyBarSlider.value = Character.Energy;
                StartCoroutine(UIManager.SetActiveBars());
            }
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        Time.timeScale = 1;
        KeysLoad();
    }

    private void KeysChange(int id)
    {
        Event e = Event.current;

        if (e.isKey)
        {
            UIManager.ButtonTexts[id].text = e.keyCode.ToString();
        }
    }

    private void KeysLoad()
    {
        keycaps = new List<string>();

        for (int i = 0; i < KeySettings.Keys.Count; i++)
        {
            keycaps.Add(KeySettings.Keys[i].CurrentKey.ToString());
        }

        for (int i = 0; i < UIManager.Toggles.Count; i++)
        {
            keycaps.Add(UIManager.ButtonTexts[i].text);
        }
    }

    private void KeysDefaultSetting()
    {
        KeySettings.KeysReset();

        for (int i = 0; i < KeySettings.Keys.Count; i++)
        {
            UIManager.ButtonTexts[i].text = KeySettings.Keys[i].CurrentKey.ToString();
        }
    }

    #endregion

    #region Events

    public void OnSetPauseButtonClicked()
    {
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
        isPause = true;
    }

    public void OnSetPlayButtonClicked()
    {
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(false);
        PanelPauseObject.SetActive(true);
        PanelClipObject.SetActive(true);
        UIManager.GunSlotObject.SetActive(true);
        isPause = false;
    }

    public void OnSetLobyButtonClicked()
    {
        CharacterData = null;
        SceneManager.LoadScene(TAG_LOBBY);
    }

    public void OnSetSettingButtonClicked()
    {
        PanelPauseObject.SetActive(false);
        PanelClipObject.SetActive(false);
        UIManager.GunSlotObject.SetActive(false);
        PauseMenuObject.SetActive(false);
        PanelSettingObject.SetActive(true);
        isPause = true;
    }

    public void OnSetDefaultKeysButtonClicked()
    {
        KeysDefaultSetting();
    }

    public void OnSetSaveKeysButtonClicked()
    {
        int Savingkeys = 0;

        for (int x = 0; x < keycaps.Count; x++)
        {
            for (int y = 0; y < keycaps.Count; y++)
            {
                if (keycaps[x] == UIManager.ButtonTexts[y].text)
                {
                    Savingkeys++;
                }
            }
        }

        if (Savingkeys == keycaps.Count - 1)
        {
            for (int i = 0; i < KeySettings.Keys.Count; i++)
            {
                KeySettings.Keys[i].CurrentKey = (KeyCode)Enum.Parse(typeof(KeyCode), UIManager.ButtonTexts[i].text);
            }
        }
        else
        {
            KeySettings.KeysReset();
        }
    }

    #endregion
}